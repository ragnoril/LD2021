using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAgent : MonoBehaviour
{
    public float speed;
    public Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check Pathfinding every other second?
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
