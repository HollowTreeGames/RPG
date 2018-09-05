using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class MenuManager : MonoBehaviour
{
    public Canvas menuCanvas;

    private SaveManager saveManager;
    private GameState gameState;
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

        gameState = FindObjectOfType<GameState>();
        saveManager = FindObjectOfType<SaveManager>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
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

    public void SaveGame()
    {
        Debug.Log("Saving game...");
        saveManager.SaveGame();
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
        saveManager.LoadGame();
    }
}
