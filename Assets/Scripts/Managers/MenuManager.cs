using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class MenuManager : MonoBehaviour
{
    public GameState gameState;
    public Canvas menuCanvas;
    public Button saveButton;

    private CanvasGroup menuCanvasGroup;
    
    private static bool instanceExists = false;

    void Start()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);

        menuCanvasGroup = menuCanvas.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuCanvas.enabled)
            {
                menuCanvas.enabled = false;
                gameState.pause = false;
            }
            else
            {
                menuCanvas.enabled = true;
                gameState.pause = true;
            }
        }
    }

    public void TestButton()
    {
        Debug.Log("boop");
    }
}
