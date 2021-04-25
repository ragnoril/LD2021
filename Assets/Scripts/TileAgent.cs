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
        if (!Physics.Raycast(new Vector3(X - 1, Y, -1f), new Vector3(X, Y, 1f)))
        {
            return true;
        }

        if (!Physics.Raycast(new Vector3(X + 1, Y, -1f), new Vector3(X, Y, 1f)))
        {
            return true;
        }

        if (!Physics.Raycast(new Vector3(X, Y - 1, -1f), new Vector3(X, Y, 1f)))
        {
            return true;
        }

        if (!Physics.Raycast(new Vector3(X, Y + 1, -1f), new Vector3(X, Y, 1f)))
        {
            return true;
        }

        return false;
    }

}
