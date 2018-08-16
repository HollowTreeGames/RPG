using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeCameraAndLoad : MonoBehaviour
{
    public float fadeRate = 3f;

    private string levelToLoad;
    private float startX;
    private float startY;

    private GameState gameState;
    private GameObject player;
    private Texture2D black;
    private bool fade = false;
    private float alpha = 0;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        black = new Texture2D(1, 1);
        black.SetPixel(0, 0, new Color(0, 0, 0, alpha));
        black.Apply();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
    }

    private void Update()
    {
        if (fade)
        {
            if (alpha < 1)
            {
                alpha += Time.deltaTime * fadeRate;
                if (alpha > 1)
                    alpha = 1f;
                black.SetPixel(0, 0, new Color(0, 0, 0, alpha));
                black.Apply();
            }
            else
            {
                LoadScene();
                fade = false;
            }
        }
        else
        {
            if (alpha > 0)
            {
                alpha -= Time.deltaTime * fadeRate;
                if (alpha < 0)
                    alpha = 0f;
                black.SetPixel(0, 0, new Color(0, 0, 0, alpha));
                black.Apply();
            }
        }
    }

    private void LoadScene()
    {
        player.SetActive(false);
        player.transform.position = new Vector2(startX, startY);
        SceneManager.LoadScene(levelToLoad);
        player.SetActive(true);
        gameState.pause = false;
    }

    public void StartLoad(GameObject player, string levelToLoad, float startX, float startY) {
        this.player = player;
        this.levelToLoad = levelToLoad;
        this.startX = startX;
        this.startY = startY;
        gameState.pause = true;
        fade = true;
	}
}
