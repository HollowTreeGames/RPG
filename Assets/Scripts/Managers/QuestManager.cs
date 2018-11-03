using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Enums;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    private Quest[] questList =
    {
        new Quest("Oakewood Greeting", "Hi Oakewood"),
        new Quest("Oakewood Stretch Legs", "Walk Around"),
        new Quest("Oakewood Run", "Run in a circle"),
        new Quest("Henry Pick Up Rock", "Pick up the rock"),
        new Quest("Henry Drop Rock", "Drop the rock"),
        new Quest("Henry Talk to Rock", "Talk to the rock")
    };

    private QuestCanvas questCanvas;
    private Canvas canvas;
    private Text questText;
    private GameState gameState;

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
        questCanvas = FindObjectOfType<QuestCanvas>();
        canvas = questCanvas.GetComponent<Canvas>();
        questText = questCanvas.GetComponentInChildren<Text>();
        gameState = FindObjectOfType<GameState>();

        if (questList.Length == 0)
        {
            CopyQuestList(QuestLoader.GetQuestList(SceneManager.GetActiveScene().name));
        }
    }

    public Quest[] GetQuestList()
    {
        return questList;
    }

    public void SetQuestList(Quest[] questList)
    {
        this.questList = questList;
    }

    public void LoadQuestsByScene()
    {
        CopyQuestList(QuestLoader.GetQuestList(SceneManager.GetActiveScene().name));
    }

    public void CopyQuestList(Quest[] newQuestList)
    {
        questList = new Quest[newQuestList.Length];
        newQuestList.CopyTo(questList, 0);
    }

    public Quest FindQuest(string id)
    {
        foreach (Quest quest in questList)
        {
            if (quest.GetQuestId() == id)
                return quest;
        }
        throw new System.MissingMemberException("No quest found with the id: " + id);
    }

    public void StartQuest(Quest quest)
    {
        quest.SetQuestState(QuestState.InProgress);
        SetQuestText(quest);
    }

    public void FinishQuest(Quest quest)
    {
        quest.Finish(gameState);
        SetQuestText(null);
    }

    public void SetQuestText(Quest quest)
    {
        if (quest == null)
        {
            questText.text = "";
            canvas.enabled = false;
            return;
        }

        questText.text = quest.GetQuestName();
        canvas.enabled = true;
    }

    void Update()
    {
        foreach (Quest quest in questList)
        {
            quest.CheckPreReqs(gameState);
        }
    }
}

