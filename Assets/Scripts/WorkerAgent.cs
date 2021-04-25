using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerStates
{
    Idle,
    Working,
    Hungry,
    Sleepy,
    Moody,
    TotalCount
};

public class WorkerAgent : MonoBehaviour
{
    Task workTask;
    bool isReadyToWork;
    public float speed;

    public WorkerStates Status;
    Coroutine moveReq;
    // Start is called before the first frame update
    void Start()
    {
        Status = WorkerStates.Idle;
        isReadyToWork = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check Pathfinding every other second?

        DoAI(Time.deltaTime);

    }

    void DoAI(float dt)
    {
        CheckForNeeds();

        if (Status == WorkerStates.Idle)
        {
            Debug.Log(gameObject.name + " waiting for new job");
            // get a new job
            int taskId = GameManager.instance.Tasks.GetAvailableTask(this);
            if (taskId != -1)
            {
                Debug.Log(gameObject.name + " got a new job with id: " + taskId.ToString());
                workTask = GameManager.instance.Tasks.TaskList[taskId];
                Status = WorkerStates.Working;
                int sX = Mathf.FloorToInt(transform.position.x);
                int sY = Mathf.FloorToInt(-transform.position.y);
                isReadyToWork = false;
                StartCoroutine(Move(GameManager.instance.StartPathFinding(sX, sY, workTask.X, workTask.Y)));
            }
        }
        else if (Status == WorkerStates.Working)
        {
            if (isReadyToWork)
            {
                Debug.Log(gameObject.name + " has new job");
            }

        }
        else if (Status == WorkerStates.Hungry)
        {
            // get a new job
        }
        else if (Status == WorkerStates.Sleepy)
        {
            // get a new job
        }
        else if (Status == WorkerStates.Moody)
        {
            // get a new job
        }
    }

    void CheckForNeeds()
    {
        ///Status = WorkerStates.Hungry;
    }

    public void MoveRequest(Vector3 targetPos)        //A move request will be sent via this
    {
        if (moveReq != null) StopCoroutine(moveReq);
        GetPathToTask(targetPos);
    }
    void GetPathToTask(Vector3 targetPos)
    {
        List<Vector3> path = GameManager.instance.StartPathFinding((int)transform.position.x, (int)transform.position.y, (int)targetPos.x, (int)targetPos.y);
        moveReq = StartCoroutine(Move(path));
    }


    IEnumerator Move(List<Vector3> path)
    {
        if (path.Count > 0)
        {
            Vector3 lastPos = path[path.Count - 1];//in case we need lastPos later
            path.RemoveAt(path.Count - 1);
        }

        foreach (Vector3 pos in path)
        {
            while (Vector3.Distance(transform.position, pos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
                yield return null;
            }

        }

        isReadyToWork = true;
    }

}
