using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameModes
{
    Dig = 0,
    Build,
    TotalCount
};


public enum BuildingNames
{
    Beds = 0,
    DiningRoom,
    Pub,
    Reactor,
    Storage,
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

    public GameObject[] WorkerPrefabs;

    public List<WorkerAgent> Workers;
    public int StartingWorkerCount;

    public GameModes GameMode;
    public TileAgent SelectedTile;

    public GameObject[] BuildingPrefabs;
    public int SelectedBuildingId;
    public BuildingAgent TempPlacementBuilding;
    public Vector2 BuildingPlacementPosition;

    public List<BuildingAgent> BuildingList;

    public int OreAmount;

    public AStar PathFinder;

    // Start is called before the first frame update
    void Start()
    {
        GameMode = GameModes.Dig;
        Level.GenerateLevel();
        PathFinder = new AStar();
        PathFinder.isDiagonalMovementAllowed = false;
        PathFinder.isNodeCostEnabled = false;

        BuildingList = new List<BuildingAgent>();
        CreateWorkers();

        DayCycle.IsRunning = true;

        UI.UpdateStatsUI();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameMode == GameModes.Build)
        {
            if (TempPlacementBuilding != null)
            {
                Vector3 screenPosition = Input.mousePosition;
                screenPosition.z = 10f; // Camera.main.transform.position.y - TempPlacementBuilding.transform.position.y;

                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);// - Camera.main.transform.position;
                //transform.position = worldPosition;
                //worldPosition.y -= Camera.main.transform.position.y;
                worldPosition.z = 0f;

                BuildingPlacementPosition = new Vector2(Mathf.Round(worldPosition.x), Mathf.Floor(worldPosition.y));
                TempPlacementBuilding.transform.localPosition = new Vector3(BuildingPlacementPosition.x, BuildingPlacementPosition.y, 0f);
                
                //Debug.Log("pso: " + worldPosition);
            }
        }

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
            if (SelectedTile.CheckIfAvailable())
            {
                int taskId = Tasks.CheckTaskListForDuplicate(SelectedTile.X, SelectedTile.Y, 0, 0);
                if (taskId == -1)
                {
                    Debug.Log("new dig task added.");
                    GameManager.instance.SfxPlayer.PlaySfx(Random.Range(7,9));
                    Tasks.AddNewTask(SelectedTile.X, SelectedTile.Y, 0, SelectedTile);
                }
                else
                {
                    Tasks.RemoveTask(taskId);
                    GameManager.instance.SfxPlayer.PlaySfx(9);
                }
            }
        }
    }

    void BuildAction()
    {
        if (TempPlacementBuilding != null)
        {
            if (OreAmount >= TempPlacementBuilding.BuiltCost)
            {
                if (TempPlacementBuilding.CheckIfAvailable())
                {
                    int taskId = Tasks.CheckTaskListForDuplicate((int)BuildingPlacementPosition.x, -(int)BuildingPlacementPosition.y, 1, SelectedBuildingId);
                    if (taskId == -1)
                    {
                        //Debug.Log("new dig task added.");
                        GameManager.instance.SfxPlayer.PlaySfx(Random.Range(7, 9));
                        Tasks.AddNewTask((int)BuildingPlacementPosition.x, -(int)BuildingPlacementPosition.y, 1, SelectedBuildingId, TempPlacementBuilding.BuiltCost);
                        CleanTempBuilding();
                    }
                }
                else
                {
                    //Debug.Log("not empty");
                    CleanTempBuilding();
                }
            }
            else 
            {
                //Debug.Log("no money");
                CleanTempBuilding();
            }
        }
    }

    public void PlaceBuilding(int buildingId, int x, int y)
    {
        GameObject go = GameObject.Instantiate(BuildingPrefabs[buildingId]);
        BuildingAgent building = go.GetComponent<BuildingAgent>();
        building.transform.SetParent(this.transform);
        building.transform.localPosition = new Vector3(x, -y, 0f);
        building.BuildIt();
        BuildingList.Add(building);

        UI.UpdateStatsUI();
    }

    public void PrepareBuilding(int buildingId)
    {
        GameObject go = GameObject.Instantiate(BuildingPrefabs[buildingId]);
        TempPlacementBuilding = go.GetComponent<BuildingAgent>();
        TempPlacementBuilding.transform.SetParent(this.transform);
        SelectedBuildingId = buildingId;
    }

    public void CleanTempBuilding()
    {
        Destroy(TempPlacementBuilding.gameObject);
        TempPlacementBuilding = null;
        SelectedBuildingId = -1;
    }


    void CreateWorkers()
    {
        Workers = new List<WorkerAgent>();
        for (int i = 0; i < StartingWorkerCount; i++)
        {
            GameObject go = GameObject.Instantiate(WorkerPrefabs[Random.Range(0, WorkerPrefabs.Length)], new Vector3(3f+i, 0f, 0), Quaternion.identity);
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
                    var layerMask = LayerMask.GetMask("Worker") | LayerMask.GetMask("Building");
                    layerMask = ~layerMask;
                    if (Physics.Raycast(new Ray(new Vector3(i, -j, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
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

    public int GetEnergySupplyAmount()
    {
        int amount = 0;
        foreach(BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 3)
            {
                amount += building.Value;
            }
        }

        return amount;
    }

    public int GetEnergyDrainAmount()
    {
        int amount = 0;
        foreach (BuildingAgent building in BuildingList)
        {
            amount += building.EnergyCost;
        }

        return amount;
    }

    public int GetBedSupplyAmount()
    {
        int amount = 0;
        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 0)
            {
                amount += building.Value;
            }
        }

        return amount;
    }

    public int GetDinerCapacity()
    {
        int amount = 0;
        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 1)
            {
                amount += building.Value;
            }
        }

        return amount;
    }

    public int GetEntertainmentCapacity()
    {
        int amount = 0;
        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 2)
            {
                amount += building.Value;
            }
        }

        return amount;
    }

    public int GetOreStorageCapacity()
    {
        int amount = 200;
        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 4)
            {
                amount += building.Value;
            }
        }

        return amount;
    }

    public BuildingAgent GetNearestDiner(Vector3 pos)
    {
        BuildingAgent agent = null;
        float dist = 99999f;

        //Vector3 pos = new Vector3(x, y, 0f);

        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 1)
            {
                if (Mathf.Abs(Vector3.Distance(building.transform.position, pos)) < dist)
                {
                    if (building.Users.Count < building.Value)
                    {
                        dist = Mathf.Abs(Vector3.Distance(building.transform.position, pos));
                        agent = building;
                    }
                }
            }
        }

        return agent;
    }

    public BuildingAgent GetNearestBed(Vector3 pos)
    {
        BuildingAgent agent = null;
        float dist = 99999f;

        //Vector3 pos = new Vector3(x, y, 0f);

        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 0)
            {
                if (Mathf.Abs(Vector3.Distance(building.transform.position, pos)) < dist)
                {
                    if (building.Users.Count < building.Value)
                    {
                        dist = Mathf.Abs(Vector3.Distance(building.transform.position, pos));
                        agent = building;
                    }
                }
            }
        }

        return agent;
    }

    public BuildingAgent GetNearestPub(Vector3 pos)
    {
        BuildingAgent agent = null;
        float dist = 99999f;

        //Vector3 pos = new Vector3(x, y, 0f);

        foreach (BuildingAgent building in BuildingList)
        {
            if (building.BuildingType == 2)
            {
                if (Mathf.Abs(Vector3.Distance(building.transform.position, pos)) < dist)
                {
                    if (building.Users.Count < building.Value)
                    {
                        dist = Mathf.Abs(Vector3.Distance(building.transform.position, pos));
                        agent = building;
                    }
                }
            }
        }

        return agent;
    }

}
