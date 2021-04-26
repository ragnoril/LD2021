using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public int X, Y;
    public int Type; // 0 dig, 1 build
    public int Value; // building id
    public int Status; // 0 waiting 1 claimed

    public GameObject TaskIcon;
    public WorkerAgent Claimant;

    public TileAgent TaskTile;
    public GameObject BuildingPrefab;

    public bool IsSame(int x, int y, int type, int value)
    {
        if (x == X && y == Y && type == Type && value == Value)
            return true;
        else
            return false;
    }

    public bool CheckIfAvailable()
    {
        // if building check if we have enough resources
        //
        var layerMask = LayerMask.GetMask("Worker");
        layerMask = ~layerMask;

        // for dig or building
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

    public int AddNewTask(int x, int y, int type, int value)
    {
        Task task = new Task();
        task.X = x;
        task.Y = y;
        task.Type = type;
        task.Value = value;
        task.Status = 0;
        task.TaskIcon = GameObject.Instantiate(TaskIconPrefab, new Vector3(x, -y, 0f), Quaternion.identity);
        task.TaskIcon.transform.SetParent(this.transform);

        TaskList.Add(task);
        return TaskList.Count - 1;
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
        
        if (TaskList[id].Claimant != null)
        {
            TaskList[id].Claimant.Status = WorkerStates.Idle;
        }

        Destroy(TaskList[id].TaskIcon);
        TaskList.RemoveAt(id);
    }

}
