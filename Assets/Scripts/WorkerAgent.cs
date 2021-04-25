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
    public float speed;

    public WorkerStates Status;
    Coroutine moveReq;
    // Start is called before the first frame update
    void Start()
    {
        Status = WorkerStates.Idle;
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
            // get a new job
        }
        else if (Status == WorkerStates.Working)
        {
            // get a new job
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
            while (transform.position != pos)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
                yield return 0;
            }

        }

    }

}
