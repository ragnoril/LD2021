using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerStates
{
    Idle,
    Working,
    Needy,
    TotalCount
};

public class WorkerAgent : MonoBehaviour
{
    Task workTask;
    bool isReadyToWork, isDead;
    public float moveSpeed, digSpeed, buildTime;

    public WorkerStates Status;
    Coroutine moveReq;
    int currentTaskID;
   // public TaskManager Tasks;

    public int energy, fun, hunger;
    public int energyThreshold, funThreshold, hungerThreshold;
    public int energyMax, funMax, hungerMax;

    // Start is called before the first frame update
    void Start()
    {
        Status = WorkerStates.Idle;
        isReadyToWork = true;

        energy = energyMax;
        fun = funMax;
        hunger = hungerMax;

        GameManager.instance.DayCycle.OnPeriodComplete += GettingTired;
    }

    private void OnDestroy()
    {
        GameManager.instance.DayCycle.OnPeriodComplete -= GettingTired;
    }

    // Update is called once per frame
    void Update()
    {
        DoAI(Time.deltaTime);
    }

    public void Eat()
    {
        hunger = hungerMax;
        isReadyToWork = true;
    }

    public void Sleep()
    {
        energy = energyMax;
        isReadyToWork = true;
    }

    public void Drink()
    {
        fun = funMax;
        isReadyToWork = true;
    }

    void GettingTired()
    {
        energy -= 1;
        hunger -= 1;
        fun -= 1;

        CheckForNeeds();
    }

    void DoAI(float dt)
    {
        if (Status == WorkerStates.Needy)
        {
            SatisfyNeeds();
        }
        else if (Status == WorkerStates.Idle)
        {
            //if (needCheckCounter == 0) CheckForNeeds();
            if (Status == WorkerStates.Idle) //if worker's still idle, check job
            {
                GameManager.instance.Tasks.AddWorker(this);
            }

        }
        else if (Status == WorkerStates.Working)
        {
            if (isReadyToWork)
                Status = WorkerStates.Idle;
        }
    }

    public void AssignTask(int taskId)
    {
        currentTaskID = taskId;
        if (taskId != -1)
        {
            //Debug.Log(gameObject.name + " got a new job with id: " + taskId.ToString());
            workTask = GameManager.instance.Tasks.TaskList[taskId];
            Status = WorkerStates.Working;
            isReadyToWork = false;
            GameManager.instance.SfxPlayer.PlaySfx(13);
            StartOrUpdatePathFinding();
        }
    }

    void StartOrUpdatePathFinding()
    {
        int sX = Mathf.FloorToInt(transform.position.x);
        int sY = -Mathf.FloorToInt(transform.position.y);
        if (moveReq != null) StopCoroutine(moveReq);
        Debug.Log("current Pos:"+sX + "," + sY + "\n Target Pos:" + workTask.X + "," + workTask.Y);
        moveReq = StartCoroutine(Move(GameManager.instance.StartPathFinding(sX, sY, workTask.X, workTask.Y)));
    }

    void GoSatisfy(BuildingAgent building)
    {
        int sX = Mathf.FloorToInt(transform.position.x);
        int sY = -Mathf.FloorToInt(transform.position.y);

        int gX = Mathf.FloorToInt(building.transform.position.x);
        int gY = -Mathf.FloorToInt(building.transform.position.y);

        StartCoroutine(MoveForNeeds(GameManager.instance.StartPathFinding(sX, sY, gX, gY), building));
    }

    void SatisfyNeeds()
    {
        if (hunger <= hungerThreshold)
        {
            BuildingAgent building = GameManager.instance.GetNearestDiner(this.transform.position);
            if (building != null)
            {
                isReadyToWork = false;
                Status = WorkerStates.Working;
                GoSatisfy(building);
            }
        }
        else if (energy <= energyThreshold)
        {
            BuildingAgent building = GameManager.instance.GetNearestBed(this.transform.position);
            if (building != null)
            {
                isReadyToWork = false;
                Status = WorkerStates.Working;
                GoSatisfy(building);
            }
        }
        else if (fun <= funThreshold)
        {
            BuildingAgent building = GameManager.instance.GetNearestPub(this.transform.position);
            if (building != null)
            {
                isReadyToWork = false;
                Status = WorkerStates.Working;
                GoSatisfy(building);
            }
        }
    }

    void CheckForNeeds()
    {
        if (hunger <= hungerThreshold)
        {
            Status = WorkerStates.Needy;
            GameManager.instance.Tasks.RemoveWorker(this);
        }
        else if (energy <= energyThreshold)
        {
            Status = WorkerStates.Needy;
            GameManager.instance.Tasks.RemoveWorker(this);
        }
        else if (fun <= funThreshold)
        {
            Status = WorkerStates.Needy;
            GameManager.instance.Tasks.RemoveWorker(this);
        }

        if (hunger == 0 || energy == 0)
        {
            //worker dead
            KillMe();
        }
    }

    void KillMe()
    {
        GameManager.instance.Workers.Remove(this);
        GameManager.instance.UI.UpdateStatsUI();
        Destroy(gameObject);
    }

    IEnumerator MoveForNeeds(List<Vector3> path, BuildingAgent building)
    {
        foreach (Vector3 pos in path)
        {
            while (Vector3.Distance(transform.position, pos) > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        if (building.Users.Count < building.Value)
            building.Use(this);
        else
            Status = WorkerStates.Needy;

    }

    IEnumerator Move(List<Vector3> path)
    {
        //Debug.Log("Moving Started");
        Vector3 lastPos=Vector3.zero;
        if (path.Count > 0)
        {
            lastPos = path[path.Count - 1];//in case we need lastPos later
            path.RemoveAt(path.Count - 1);
        }

        foreach (Vector3 pos in path)
        {
            while (Vector3.Distance(transform.position, pos) > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
                yield return null;
            }

        }
        if (workTask.Type == 0)
        {
            //dig'e basla 
            //Debug.Log("Moving Ended");
            StartCoroutine(Digging(lastPos));
        }
        else
        {
            //build'e basla
            //Debug.Log("Moving Ended");
            if (GameManager.instance.OreAmount >= workTask.Cost)
            {
                GameManager.instance.OreAmount -= workTask.Cost;
                StartCoroutine(Building(lastPos));
            }
            else
            {
                workTask.Claimant = null;
                workTask.Status = 0;
                workTask = null;
                if (!CheckIfFloating())
                    isReadyToWork = true;
            }
            float timer = 0;
            while (timer<buildTime)
            {
                yield return null;
            }
        }
    }

    IEnumerator Building(Vector3 buildPos)
    {
        yield return new WaitForSeconds(buildTime);
        GameManager.instance.SfxPlayer.PlaySfx(0);
        GameManager.instance.PlaceBuilding(workTask.Value, workTask.X, workTask.Y);

        FinishTask(currentTaskID);
    }

    IEnumerator Digging(Vector3 digPos)
    {
        //Debug.Log("Digging Started");
        float i = 0;
        bool played = false;
        GameManager.instance.SfxPlayer.PlaySfx(4);
        while (Vector3.Distance(transform.position, digPos) > 0)
        {
            i += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, digPos, digSpeed * Time.deltaTime);
            if (i>1 && !played)
            {
                GameManager.instance.SfxPlayer.PlaySfx(4);
                played = true;
            }
            yield return null;
        }
        //Debug.Log("Digging Ended");
        GameManager.instance.SfxPlayer.PlaySfx(5);
        workTask.TaskTile.Dug();
        FinishTask(currentTaskID);
    }

    void FinishTask(int currentTaskID)
    {
        //GameManager.instance.Tasks.RemoveTask(currentTaskID);
        GameManager.instance.Tasks.RemoveTask(workTask);
        //Status = WorkerStates.Idle;
        isReadyToWork = true;
        //CheckForNeeds();
        if (!CheckIfFloating())
            isReadyToWork = true;
    }

    bool CheckIfFloating()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {
            if (Vector3.Distance(transform.position, hit.transform.position) > 1)
            {
                StartCoroutine(MoveFloating(hit.transform.position + Vector3.up));
                return true;
            }
        }
        return false;
    }

    IEnumerator MoveFloating(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isReadyToWork = true;
    }
}
