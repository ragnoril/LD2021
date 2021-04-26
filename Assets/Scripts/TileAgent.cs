using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAgent : MonoBehaviour
{
    public GameObject SelectionObject;

    public int X, Y;
    public int Value; // 0 dirt, 1 ore

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (GameManager.instance.SelectedTile == this)
        {
            Debug.DrawRay(new Vector3(X - 1, -Y, -1f), new Vector3(0, 0, 2f));
            Debug.DrawRay(new Vector3(X + 1, -Y, -1f), new Vector3(0, 0, 2f));
            Debug.DrawRay(new Vector3(X, -Y - 1, -1f), new Vector3(0, 0, 2f));
            Debug.DrawRay(new Vector3(X, -Y + 1, -1f), new Vector3(0, 0, 2f));
        }
        */
    }

    private void OnMouseOver()
    {
        //SelectionObject.SetActive(true);
    }

    private void OnMouseEnter()
    {
        GameManager.instance.SelectedTile = this;

        if (GameManager.instance.GameMode == GameModes.Dig)
            SelectionObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        GameManager.instance.SelectedTile = null;
        SelectionObject.SetActive(false);
    }

    public bool CheckIfAvailable()
    {
        var layerMask = LayerMask.GetMask("Building");
        if (Physics.Raycast(new Ray(new Vector3(X, -(Y - 1), -1f), new Vector3(0, 0, 3f)), 999f, layerMask))
        {
            return false;
        }

        return true;

        /*
        var layerMask = LayerMask.GetMask("Worker");
        layerMask = ~layerMask;

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
        */
    }

    public void Dug()
    {
        //+ore ekle
        GameManager.instance.OreAmount += Value;
        GameManager.instance.OreAmount = Mathf.Min(GameManager.instance.OreAmount, GameManager.instance.GetOreStorageCapacity());
        Destroy(gameObject);
    }
}
