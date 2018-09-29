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

    private float panSpeed = 0;
    private Vector2 panDirection = Vector2.zero;
    private Vector3 defaultPosition;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        black = new Texture2D(1, 1);
        black.SetPixel(0, 0, new Color(0, 0, 0, alpha));
        black.Apply();
        defaultPosition = transform.localPosition;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
    }

    private void Update()
    {
        Fade();
        PanCamera();
    }

    private void Fade()
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
        if (player != null)
        {
            player.SetActive(false);
            player.transform.position = new Vector2(startX, startY);
        }
        SceneManager.LoadScene(levelToLoad);
        if (player != null)
        {
            player.SetActive(true);
        }
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
        transform.localPosition = defaultPosition;
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
