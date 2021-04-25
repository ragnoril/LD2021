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
    public Vector3 targetPos;

    public WorkerStates Status;
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

    public void MoveX(int tile)
    {
        targetPos = transform.position + new Vector3(tile, 0, 0);
    }
    public void MoveY(int tile)
    {
        targetPos = transform.position + new Vector3(0, tile, 0);
    }

    public void MoveRequest(Vector3 targetPos)        //A move request will be sent via this
    {
        this.targetPos = targetPos;
        //Add pathfingind here ie: pathfinding(currentPos, targetPos)//output can be a vector
        //Ex: ((-1,3),(1,5),(-2,4),(2,3)) -> -1:left, 1:right, -2:down, 2:up, (-1,3): 3 tiles left, (2,3): 3 tiles up

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {

        //foreach loop for each Pathfinding positions
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        yield return 0;

    }

    void GetPathToTask()
    {
        // worker'in coordinatlar
        // task coords
        // return path
        // ignore last coord
        //GameManager.instance.StartPathFinding(, Y, 0, 0);
    }
}
