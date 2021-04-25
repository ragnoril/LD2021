using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameModes
{
    Dig = 0,
    Build,
    TotalCount
};

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    public MusicManager MusicPlayer;
    public SfxManager SfxPlayer;
    public LevelManager Level;
    public UIManager UI;
    public TaskManager Tasks;
    public DayCycleManager DayCycle;

    public GameObject WorkerPrefab;

    public List<WorkerAgent> Workers;
    public int StartingWorkerCount;

    public GameModes GameMode;
    public TileAgent SelectedTile;

    public AStar PathFinder;

    // Start is called before the first frame update
    void Start()
    {
        GameMode = GameModes.Dig;
        Level.GenerateLevel();
        PathFinder = new AStar();
        PathFinder.isDiagonalMovementAllowed = false;
        PathFinder.isNodeCostEnabled = false;

        CreateWorkers();

        DayCycle.IsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameMode == GameModes.Dig)
            {
                DigAction();
            }
            else if (GameMode == GameModes.Build)
            {
                BuildAction();
            }
        }
    }

    void DigAction()
    {
        if (SelectedTile != null)
        {
            // check if there is a building above selected tile
            //if (SelectedTile.CheckIfAvailable())
            {
                int taskId = Tasks.CheckTaskListForDuplicate(SelectedTile.X, SelectedTile.Y, 0, 0);
                if (taskId == -1)
                {
                    Debug.Log("new dig task added.");
                    Tasks.AddNewTask(SelectedTile.X, SelectedTile.Y, 0, SelectedTile);
                }
                else
                {
                    Tasks.RemoveTask(taskId);
                }
            }
        }
    }

    void BuildAction()
    {

    }

    void CreateWorkers()
    {
        Workers = new List<WorkerAgent>();
        for (int i = 0; i < StartingWorkerCount; i++)
        {
            GameObject go = GameObject.Instantiate(WorkerPrefab, new Vector3(3f+i, 0f, 0), Quaternion.identity);
            go.transform.SetParent(this.transform);

            go.name = "Worker_" + i.ToString();
            WorkerAgent worker = go.GetComponent<WorkerAgent>();
            Workers.Add(worker);

        }
    }

    List<int> GenerateTileMap(int gX, int gY)
    {
        List<int> updatedList = new List<int>();

        for (int j = 0; j < Level.Height; j++)
        {
            for (int i = 0; i < Level.Width; i++)
            {
                if (i == gX && j == gY)
                {
                    updatedList.Add(1);
                }
                else
                {
                    var layerMask = LayerMask.GetMask("Worker");
                    layerMask = ~layerMask;
                    if (Physics.Raycast(new Ray(new Vector3(i, -j, -1f), new Vector3(0, 0, 3f)),999f, layerMask))
                    {
                        updatedList.Add(0);
                    }
                    else
                    {
                        updatedList.Add(1);
                    }
                }
            }
        }


        return updatedList;
    }

    List<Vector3> GeneratePath()
    {
        List<Vector3> path = new List<Vector3>();

        int lastPath = PathFinder.finalPath.Count - 1;
        for (int i = 0; i < PathFinder.finalPath.Count; i++)
        {
            Node node = PathFinder.finalPath[PathFinder.finalPath.Count - i - 1];
            path.Add(new Vector3(node.x, -node.y, 0f));
        }

        return path;
    }

    public List<Vector3> StartPathFinding(int sX, int sY, int gX, int gY)
    {
        PathFinder.SetMapSize(Level.Width, Level.Height);
        PathFinder.SetStartNode(sX, sY);
        PathFinder.SetGoalNode(gX, gY);
        PathFinder.StartSearch(GenerateTileMap(gX, gY));
        PathFinder.GetPath();

        return GeneratePath();
    }
}
