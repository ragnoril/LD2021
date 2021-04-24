using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanelManager : MonoBehaviour, IPointerExitHandler
{
    bool mouseOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mouseOver)
        {
            CloseUI();
        }
    }

    public void SetMouseOver(bool b)
    {
        mouseOver = b;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //CloseUI();
    }

    void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
