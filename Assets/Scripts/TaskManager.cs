using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public int X, Y;
    public int Type; // 0 dig, 1 build
    public int Value; // building id
    public int Status; // 0 waiting 1 claimed
    public int Cost;

    public GameObject TaskIcon;
    public WorkerAgent Claimant;

    public TileAgent TaskTile;

    public bool IsSame(int x, int y, int type, int value)
    {
        if (x == X && y == Y && type == Type && value == Value)
            return true;
        else
            return false;
    }

    public bool CheckIfAvailable()
    {
        if (Type == 0)
            return CheckIfAvailableForDig();
        else if (Type == 1)
            return CheckIfAvailableForBuilding();
        else
            return false; // shouldn't happen.
    }

    public bool CheckIfAvailableForDig()
    {
        // if building check if we have enough resources
        //
        var layerMask = LayerMask.GetMask("Worker");
        layerMask = ~layerMask;

        // for dig
        if (!Physics.Raycast(new Ray(new Vector3(X - 1, -Y, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("west of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X + 1, -Y, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("east of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X, -(Y - 1), -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("south of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X, -(Y + 1), -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("north of it empty");
            return true;
        }

        //Debug.Log("no empty");

        return false;
    }

    public bool CheckIfAvailableForBuilding()
    {
        // if building check if we have enough resources
        if (GameManager.instance.OreAmount < Cost)
            return false;

        var layerMask = LayerMask.GetMask("Worker");
        layerMask = ~layerMask;

        // for dig
        if (!Physics.Raycast(new Ray(new Vector3(X - 1, -Y, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("west of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X + 1, -Y, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("east of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X, -(Y - 1), -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("south of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X, -(Y + 1), -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            //Debug.Log("north of it empty");
            return true;
        }

        //Debug.Log("no empty");

        return false;
    }
}

public class TaskManager : MonoBehaviour
{
    public List<Task> TaskList = new List<Task>();
    public GameObject TaskIconPrefab;
    public GameObject[] TaskBuildingIconPrefabs;

    public List<WorkerAgent> AvailableWorkers;

    private void Start()
    {
        AvailableWorkers = new List<WorkerAgent>();

        GameManager.instance.DayCycle.OnClockTicks += AssignTasks;
    }

    private void OnDestroy()
    {
        if (GameManager.instance.DayCycle != null)
            GameManager.instance.DayCycle.OnClockTicks -= AssignTasks;
    }

    void AssignTasks()
    {
        if (AvailableWorkers.Count < 1)
            return;

        for (int i = 0; i < TaskList.Count; i++)
        {
            if (TaskList[i].Status == 0 && TaskList[i].CheckIfAvailable())
            {
                WorkerAgent agent = GetNearestWorker(TaskList[i].X, TaskList[i].Y);
                if (agent != null)
                {
                    TaskList[i].Status = 1;
                    TaskList[i].Claimant = agent;
                    RemoveWorker(agent);
                    TaskList[i].Claimant.AssignTask(i);
                }
            }
        }
    }

    public int AddNewTask(int x, int y, int type, TileAgent tile)
    {
        Task task = new Task();
        task.X = x;
        task.Y = y;
        task.Type = type;
        task.Value = 0;
        task.Status = 0;
        task.TaskTile = tile;
        task.TaskIcon = GameObject.Instantiate(TaskIconPrefab, new Vector3(x, -y, 0f), Quaternion.identity);
        task.TaskIcon.transform.SetParent(this.transform);

        TaskList.Add(task);

        return TaskList.Count - 1;
    }

    public int AddNewTask(int x, int y, int type, int value, int cost)
    {
        Task task = new Task();
        task.X = x;
        task.Y = y;
        task.Type = type;
        task.Value = value;
        task.Status = 0;
        task.Cost = cost;

        if (type == 0)
            task.TaskIcon = GameObject.Instantiate(TaskIconPrefab, new Vector3(x, -y, 0f), Quaternion.identity);
        else
            task.TaskIcon = GameObject.Instantiate(TaskBuildingIconPrefabs[value], new Vector3(x, -y, 0f), Quaternion.identity);
        task.TaskIcon.transform.SetParent(this.transform);
        

        TaskList.Add(task);
        return TaskList.Count - 1;
    }

    public int GetTaskAtXY(int x, int y)
    {
        for (int i = 0; i < TaskList.Count; i++)
        {
            if (TaskList[i].X == x && TaskList[i].Y == y)
                return i;
        }
        return -1;
    }

    public int CheckTaskListForDuplicate(int x, int y, int type, int value)
    {
        for (int i = 0; i < TaskList.Count; i++)
        {
            if (TaskList[i].IsSame(x, y, type, value))
                return i;
        }
        return -1;
    }

    public int GetAvailableTask(WorkerAgent worker)
    {
        for(int i = 0; i < TaskList.Count; i++)
        {
            if (TaskList[i].Status == 0 && TaskList[i].CheckIfAvailable())
            {
                TaskList[i].Status = 1;
                TaskList[i].Claimant = worker;
                return i;
            }
        }

        return -1;
    }

    public void RemoveTask(int id)
    {
        if (TaskList[id].Claimant != null)
        {
            TaskList[id].Claimant.Status = WorkerStates.Idle;
        }
        Destroy(TaskList[id].TaskIcon);
        TaskList.RemoveAt(id);
    }

    public void RemoveTask(Task task)
    {
        int id = TaskList.IndexOf(task);
        if (TaskList[id] != null)
        {
            if (TaskList[id].Claimant != null)
            {
                TaskList[id].Claimant.Status = WorkerStates.Idle;
            }

            Destroy(TaskList[id].TaskIcon);
            TaskList.RemoveAt(id);
        }
    }

    public void AddWorker(WorkerAgent workerAgent)
    {
        if (AvailableWorkers.Contains(workerAgent) == false)
        {
            AvailableWorkers.Add(workerAgent);
        }
    }

    public void RemoveWorker(WorkerAgent workerAgent)
    {
        if (AvailableWorkers.Contains(workerAgent) == true)
        {
            AvailableWorkers.Remove(workerAgent);
        }
    }

    public WorkerAgent GetNearestWorker(int x, int y)
    {
        Vector3 taskPos = new Vector3(x, -y, 0f);
        WorkerAgent agent = null;
        float dist = 99999f;
        foreach(WorkerAgent worker in AvailableWorkers)
        {
            if (Mathf.Abs(Vector3.Distance(worker.transform.position, taskPos)) < dist)
            {
                dist = Mathf.Abs(Vector3.Distance(worker.transform.position, taskPos));
                agent = worker;
            }
        }

        return agent;
    }
}
