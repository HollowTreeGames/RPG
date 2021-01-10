using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class ButtonManager : MonoBehaviour
{
    public GameState gameState;
    public SaveManager saveManager;
    public SceneLoader sceneLoader;

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
        sceneLoader.StartLoad(gameState.currentScene, gameState.startPosition.x, gameState.startPosition.y);
    }
}
