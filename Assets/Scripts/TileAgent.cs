using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAgent : MonoBehaviour
{
    public GameObject SelectionObject;

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
        if (GameManager.instance.GameMode == GameModes.Dig)
            SelectionObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        SelectionObject.SetActive(false);
    }
}
