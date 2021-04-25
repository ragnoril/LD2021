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
        if (GameManager.instance.SelectedTile == this)
        {
            Debug.DrawRay(new Vector3(X - 1, -Y, -1f), new Vector3(0, 0, 2f));
            Debug.DrawRay(new Vector3(X + 1, -Y, -1f), new Vector3(0, 0, 2f));
            Debug.DrawRay(new Vector3(X, -Y - 1, -1f), new Vector3(0, 0, 2f));
            Debug.DrawRay(new Vector3(X, -Y + 1, -1f), new Vector3(0, 0, 2f));
        }
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
        if (!Physics.Raycast(new Ray(new Vector3(X - 1, -Y, -1f), new Vector3(0, 0, 3f))))
        {
            //Debug.Log("west of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X + 1, -Y, -1f), new Vector3(0, 0, 3f))))
        {
            //Debug.Log("east of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X, -(Y - 1), -1f), new Vector3(0, 0, 3f))))
        {
            //Debug.Log("south of it empty");
            return true;
        }

        if (!Physics.Raycast(new Ray(new Vector3(X, -(Y + 1), -1f), new Vector3(0, 0, 3f))))
        {
            //Debug.Log("north of it empty");
            return true;
        }

        //Debug.Log("no empty");

        return false;
    }

}
