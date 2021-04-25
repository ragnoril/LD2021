using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public int X, Y;
    public int Type; // 0 dig, 1 build
    public int Value; // building id
    public int Status; // 0 waiting 1 claimed
}

public class TaskManager : MonoBehaviour
{
    public List<Task> TaskList = new List<Task>();


    public int AddNewTask(int x, int y, int type, int value)
    {
        Task task = new Task();
        task.X = x;
        task.Y = y;
        task.Type = type;
        task.Value = value;
        task.Status = 0;

        TaskList.Add(task);

        return TaskList.Count - 1;
    }

    public int GetAvailableTask()
    {
        for(int i = 0; i < TaskList.Count; i++)
        {
            if (TaskList[i].Status == 0)
            {
                TaskList[i].Status = 1;
                return i;
            }
        }

        return -1;
    }

    public void RemoveTask(int id)
    {
        TaskList.RemoveAt(id);
    }

}
