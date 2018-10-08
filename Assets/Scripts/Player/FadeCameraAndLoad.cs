﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeCameraAndLoad : MonoBehaviour
{
    public float defaultFadeRate = 3f;
    public bool startBlack;

    private bool loadScene;
    private string levelToLoad;
    private float startX;
    private float startY;

    private GameState gameState;
    private Player player;
    private Texture2D black;

    private float fadeRate;
    private bool fadeOut = false;
    private float alpha = 0;

    private float panSpeed = 0;
    private Vector2 panDirection = Vector2.zero;
    private Vector3 defaultPosition;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        gameState = FindObjectOfType<GameState>();

        transform.parent = player.transform;
        defaultPosition = transform.localPosition;

        if (startBlack)
            alpha = 1;
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
        Fade();
        
        if (alpha < 1 && loadScene)
        {
            LoadScene();
            loadScene = false;
            fadeOut = false;
        }

        PanCamera();
    }

    private void Fade()
    {
        if (fadeOut)
        {
            if (alpha < 1)
            {
                alpha += Time.deltaTime * fadeRate;
                if (alpha > 1)
                    alpha = 1f;
                black.SetPixel(0, 0, new Color(0, 0, 0, alpha));
                black.Apply();
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

    public void StartLoad(string levelToLoad, float startX, float startY, float fadeRate = 0) {
        this.levelToLoad = levelToLoad;
        this.startX = startX;
        this.startY = startY;
        this.fadeRate = (fadeRate > 0) ? fadeRate : defaultFadeRate;
        gameState.pause = true;
        loadScene = true;
        fadeOut = true;
    }

    private void LoadScene()
    {
        if (player != null)
        {
            player.enabled = false;
            player.transform.position = new Vector2(startX, startY);
        }
        SceneManager.LoadScene(levelToLoad);
        if (player != null)
        {
            player.enabled = true;
        }
        gameState.pause = false;
    }

    public void FadeOut(float fadeRate = 0)
    {
        this.fadeRate = (fadeRate > 0) ? fadeRate : defaultFadeRate;
        fadeOut = true;
    }

    public void FadeIn(float fadeRate = 0)
    {
        this.fadeRate = (fadeRate > 0) ? fadeRate : defaultFadeRate;
        fadeOut = false;
    }

    [Yarn.Unity.YarnCommand("fadeOut")]
    public void YarnFadeOut()
    {
        FadeOut();
    }

    [Yarn.Unity.YarnCommand("fadeOut")]
    public void YarnFadeOut(string fadeRate)
    {
        float fFadeRate;
        try
        {
            fFadeRate = float.Parse(fadeRate);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid fade rate: {0}", fadeRate);
            return;
        }

        FadeOut(fFadeRate);
    }

    [Yarn.Unity.YarnCommand("fadeIn")]
    public void YarnFadeIn()
    {
        FadeIn();
    }

    [Yarn.Unity.YarnCommand("fadeIn")]
    public void YarnFadeIn(string fadeRate)
    {
        float fFadeRate;
        try
        {
            fFadeRate = float.Parse(fadeRate);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid fade rate: {0}", fadeRate);
            return;
        }

        FadeIn(fFadeRate);
    }

    [Yarn.Unity.YarnCommand("place")]
    public void PlaceCamera(string x, string y)
    {
        float fX;
        try
        {
            fX = float.Parse(x);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid x: {0}", x);
            return;
        }

        float fY;
        try
        {
            fY = float.Parse(y);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid y: {0}", y);
            return;
        }

        transform.localPosition = new Vector3(fX, fY, defaultPosition.z);
    }

    [Yarn.Unity.YarnCommand("move")]
    public void MoveCamera(string direction, string speed)
    {
        float fSpeed;
        try
        {
            fSpeed = float.Parse(speed);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid move speed: {0}", speed);
            return;
        }

        panDirection = Utils.ParseFacing(direction);
        panSpeed = fSpeed;
    }

    [Yarn.Unity.YarnCommand("stop")]
    public void StopCamera()
    {
        panSpeed = 0;
        panDirection = Vector2.zero;
    }

    [Yarn.Unity.YarnCommand("reset")]
    public void ResetCameraPosition()
    {
        LockCameraToPlayer();
        transform.localPosition = defaultPosition;
    }

    [Yarn.Unity.YarnCommand("free")]
    public void FreeCameraFromPlayer()
    {
        transform.parent = null;
    }

    [Yarn.Unity.YarnCommand("lock")]
    public void LockCameraToPlayer()
    {
        transform.parent = player.transform;
    }

    private void PanCamera()
    {
        if (panSpeed > 0)
        {
            float panDeltaX = transform.position.x + (panDirection.x * panSpeed * Time.deltaTime);
            float panDeltaY = transform.position.y + (panDirection.y * panSpeed * Time.deltaTime);
            transform.position = new Vector3(panDeltaX, panDeltaY, transform.position.z);
            Debug.LogFormat("Camera position: {0}", transform.position);
        }
    }
}
