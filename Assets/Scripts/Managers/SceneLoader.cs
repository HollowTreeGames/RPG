using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneLoader : MonoBehaviour
{
    private GameState gameState;
    private FadeCameraAndLoad fadeCamera;
    private Player player;
    private QuestManager questManager;
    private DialogueRunner dialogueRunner;

    private bool loadingNewScene;
    private bool sceneUnloaded;
    private bool sceneLoaded;

    private string levelToLoad;
    private float startX;
    private float startY;
    private float fadeRate;
    private bool loadNewChapter;

    private static bool instanceExists = false;

    private void Awake()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        fadeCamera = FindObjectOfType<FadeCameraAndLoad>();
        player = FindObjectOfType<Player>();
        questManager = FindObjectOfType<QuestManager>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #region LoadScene
    public void StartLoad(string levelToLoad, float startX, float startY, float fadeRate = 0, 
        bool loadNewChapter = false)
    {
        this.levelToLoad = levelToLoad;
        this.startX = startX;
        this.startY = startY;
        this.fadeRate = fadeRate;
        this.loadNewChapter = loadNewChapter;

        loadingNewScene = true;
        sceneUnloaded = false;
        sceneLoaded = true;
        gameState.pause = true;
        fadeCamera.FadeOut(fadeRate);
        StopAllCoroutines();
        StartCoroutine(WaitForFadeOut());
    }

    private IEnumerator WaitForFadeOut()
    {
        while (!fadeCamera.IsFadedOut())
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnSceneFadedOut();
    }

    private IEnumerator WaitForFadeIn()
    {
        while (!fadeCamera.IsFadedIn())
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnSceneFadedIn();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    #endregion

    #region SceneManager event handlers
    private void OnSceneUnloaded(Scene current)
    {
        sceneUnloaded = true;
        DisableAndMovePlayer();
        LoadQuests();
        if (loadNewChapter)
            dialogueRunner.dialogue.UnloadAll();
    }

    private void OnSceneLoaded(Scene current, LoadSceneMode mode)
    {
        sceneLoaded = true;
        EnablePlayer();

        fadeCamera.FadeIn(fadeRate);
        StopAllCoroutines();
        StartCoroutine(WaitForFadeIn());
    }

    private void OnSceneFadedOut()
    {
        LoadScene();
    }

    private void OnSceneFadedIn()
    {
        // Scene done loading. Reset all flags.
        gameState.pause = false;
        loadingNewScene = false;
        loadNewChapter = false;
    }
    #endregion

    #region Helper functions
    public bool IsLoadingNewScene()
    {
        return loadingNewScene;
    }

    public bool IsSceneUnloaded()
    {
        return sceneUnloaded;
    }

    public bool IsSceneLoaded()
    {
        return sceneLoaded;
    }

    private void DisableAndMovePlayer()
    {
        if (player != null)
        {
            player.enabled = false;
            player.transform.position = new Vector2(startX, startY);
        }
    }

    private void EnablePlayer()
    {
        if (player != null)
        {
            player.enabled = true;
        }
    }

    private void LoadQuests()
    {
        if (loadNewChapter)
            questManager.CopyQuestList(QuestLoader.GetQuestList(levelToLoad));
    }
    #endregion
}
