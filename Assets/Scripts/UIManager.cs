using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public void DigCommand()
    {
        GameManager.instance.GameMode = GameModes.Dig;
    }

    public void BuildCommand()
    {
        GameManager.instance.GameMode = GameModes.Build;
    }

}
