using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class ButtonManager : MonoBehaviour
{
    public FadeCameraAndLoad fadeCamera;
    public GameState gameState;
    public SaveManager saveManager;

    public void NewGame()
    {
        Debug.Log("Starting new game...");
        LoadScene();
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
        saveManager.LoadGame();
        LoadScene();
    }

    private void LoadScene()
    {
        fadeCamera.StartLoad(null, gameState.currentScene, gameState.startPosition.x, gameState.startPosition.y);
    }
}
