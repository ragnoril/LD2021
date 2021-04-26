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

    // Start is called before the first frame update
    void Start()
    {
        IsWorking = false;
    }

    public void BuildIt()
    {
        GameManager.instance.DayCycle.OnPeriodComplete += EnergyCheck;
        IsWorking = true;
    }

    private void OnDestroy()
    {
        GameManager.instance.DayCycle.OnPeriodComplete -= EnergyCheck;
    }

    void EnergyCheck()
    {

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
        RaycastHit hitInfo;
        for (int i =0; i < BuildingWidth; i++)
        {
            if (Physics.Raycast(new Ray(new Vector3(GameManager.instance.BuildingPlacementPosition.x + i, GameManager.instance.BuildingPlacementPosition.y, -1f), new Vector3(0, 0, 3f)), out hitInfo, 999f, layerMask))
            {
                if (hitInfo.collider != null)
                {
                    if (hitInfo.transform == this.transform)
                    {
                        return true;
                        //Debug.Log("hit me");
                    }
                    else
                    {
                        //Debug.Log("hit other");
                        return false;
                    }
                }
                else
                {
                    //Debug.Log("hit none");
                    return true;
                }
            }
        }
        //Debug.Log("no hit");
        collider.enabled = true;
        return true;
    }
}
