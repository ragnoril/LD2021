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
    float needCheckCounter;

    // Start is called before the first frame update
    void Start()
    {
        Status = WorkerStates.Idle;
        isReadyToWork = false;

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

    void GettingTired()
    {
        energy -= 1;
        hunger -= 1;
        fun -= 1;
    }

    void DoAI(float dt)
    {
        needCheckCounter += dt;
        if (needCheckCounter > 1) needCheckCounter = 0;
        if (Status == WorkerStates.Needy)
        {

        }
        else if (Status == WorkerStates.Idle)
        {
            if (needCheckCounter == 0) CheckForNeeds();
            if (Status == WorkerStates.Idle) //if worker's still idle, check job
            {
                //Debug.Log(gameObject.name + " waiting for new job");
                // get a new job
                int taskId = GameManager.instance.Tasks.GetAvailableTask(this);
                currentTaskID = taskId;
                if (taskId != -1)
                {
                    Debug.Log(gameObject.name + " got a new job with id: " + taskId.ToString());
                    workTask = GameManager.instance.Tasks.TaskList[taskId];
                    Status = WorkerStates.Working;
                    isReadyToWork = false;
                    StartOrUpdatePathFinding();
                }
            }

        }
        else if (Status == WorkerStates.Working)
        {
            if (isReadyToWork)
            {
                Debug.Log(gameObject.name + " has new job");
            }
            //if (needCheckCounter == 0) StartOrUpdatePathFinding();

        }
    }

    void StartOrUpdatePathFinding()
    {
        int sX = Mathf.FloorToInt(transform.position.x);
        int sY = Mathf.FloorToInt(-transform.position.y);
        if (moveReq != null) StopCoroutine(moveReq);
        moveReq = StartCoroutine(Move(GameManager.instance.StartPathFinding(sX, sY, workTask.X, workTask.Y)));
    }
    void CheckForNeeds()
    {
        if (hunger <= hungerThreshold)
        {
            Status = WorkerStates.Needy;
            isReadyToWork = false;
        }
        else if (energy <= energyThreshold)
        {
            Status = WorkerStates.Needy;
            isReadyToWork = false;
        }
        else if (fun <= funThreshold)
        {
            Status = WorkerStates.Needy;
            isReadyToWork = false;
        }
    }

    IEnumerator Move(List<Vector3> path)
    {
        Debug.Log("Moving Started");
        Vector3 lastPos=Vector3.zero;
        if (path.Count > 0)
        {
            lastPos = path[path.Count - 1];//in case we need lastPos later
            path.RemoveAt(path.Count - 1);
        }

        foreach (Vector3 pos in path)
        {
            while (Vector3.Distance(transform.position, pos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
                yield return null;
            }

        }
        if (workTask.Type == 0)
        {
            //dig'e basla 
            StartCoroutine(Digging(lastPos));
        }
        else
        {
            //build'e basla
            float timer = 0;
            while (timer<buildTime)
            {
                yield return null;
            }
        }
        Debug.Log("Moving Ended");
    }

    IEnumerator Digging(Vector3 digPos)
    {
        Debug.Log("Digging Started");
        while (Vector3.Distance(transform.position, digPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, digPos, digSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Digging Ended");
        FinishTask(currentTaskID);
    }

    void FinishTask(int currentTaskID=-5)
    {
        if (currentTaskID != -5) GameManager.instance.Tasks.RemoveTask(currentTaskID);
        Status = WorkerStates.Idle;
        isReadyToWork = true;
        CheckForNeeds();
    }

}
