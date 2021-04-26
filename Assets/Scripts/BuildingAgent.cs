using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAgent : MonoBehaviour
{
    public int BuildingWidth;
    public int BuiltCost;

    public int BuildingType;
    public int EnergyCost;
    public int Value;
    public bool IsWorking;

    public List<WorkerAgent> Users;

    // Start is called before the first frame update
    void Start()
    {
        IsWorking = false;
        Users = new List<WorkerAgent>();
    }

    public void BuildIt()
    {
        GameManager.instance.DayCycle.OnPeriodComplete += EnergyCheck;

        if (GameManager.instance.GetEnergyDrainAmount() > GameManager.instance.GetEnergySupplyAmount())
            IsWorking = false;
        else
            IsWorking = true;
    }

    private void OnDestroy()
    {
        GameManager.instance.DayCycle.OnPeriodComplete -= EnergyCheck;
    }

    void EnergyCheck()
    {
        if (IsWorking)
        {
            WorkIt();
        }
        else
        {
            if (GameManager.instance.GetEnergyDrainAmount() <= GameManager.instance.GetEnergySupplyAmount())
                IsWorking = true;
        }
    }

    void WorkIt()
    {
        for (int i = 0; i < Users.Count; i++)
        {
            if (BuildingType == 0)
            {
                Users[i].Sleep();
            }
            else if (BuildingType == 1)
            {
                Users[i].Eat();
            }
            else if (BuildingType == 2)
            {
                Users[i].Drink();
            }
        }

        Users.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfAvailable()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        var layerMask = LayerMask.GetMask("Worker");
        layerMask = ~layerMask;
        for (int i =0; i < BuildingWidth; i++)
        {
            if (Physics.Raycast(new Ray(new Vector3(GameManager.instance.BuildingPlacementPosition.x + i, GameManager.instance.BuildingPlacementPosition.y, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
            {
                return false;
            }
            else
            {
                if (!Physics.Raycast(new Ray(new Vector3(GameManager.instance.BuildingPlacementPosition.x + i, GameManager.instance.BuildingPlacementPosition.y - 1, -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
                {
                    return false;
                }
            }
        }
        //Debug.Log("no hit");
        collider.enabled = true;
        return true;
    }

    public void Use(WorkerAgent worker)
    {
        Users.Add(worker);
    }
}
