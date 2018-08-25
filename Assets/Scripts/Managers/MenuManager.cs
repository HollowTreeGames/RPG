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
            if (menuCanvasGroup.alpha == 0)
            {
                menuCanvasGroup.alpha = 1;
                gameState.pause = true;
            }
            else
            {
                menuCanvasGroup.alpha = 0;
                gameState.pause = false;
            }
        }
    }

    public void TestButton()
    {
        Debug.Log("boop");
    }
}
