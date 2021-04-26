using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRoll : MonoBehaviour
{
    public RectTransform RollTextTransform;
    private float _startPos;
    private float _resetPos;
    public float MoveSpeed;

    public bool IsPaused;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = -RollTextTransform.sizeDelta.y;
        _resetPos = RollTextTransform.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
            IsPaused = true;
        else
            IsPaused = false;

        if (!IsPaused)
        {
            Vector3 pos = RollTextTransform.localPosition;
            pos.y += MoveSpeed;

            if (pos.y > _resetPos)
            {
                pos.y = _startPos;
            }

            RollTextTransform.localPosition = pos;
        }
    }

    public void ToggleCredits()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
