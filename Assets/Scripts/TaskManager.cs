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

    public bool IsSame(int x, int y, int type, int value)
    {
        if (x == X && y == Y && type == Type && value == Value)
            return true;
        else
            return false;
    }
}

public class TaskManager : MonoBehaviour
{
    public List<Task> TaskList = new List<Task>();
    public GameObject TaskIconPrefab;

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
        Destroy(TaskList[id].TaskIcon);
        TaskList.RemoveAt(id);
    }

}
