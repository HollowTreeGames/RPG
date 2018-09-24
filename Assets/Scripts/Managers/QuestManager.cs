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
        new Quest("Oakewood Greeting", "Use space to talk to that Old Coyote")
            .StartAutomatically()
            .AddReputation(0)
            .InitialState(QuestState.Available),
        new Quest("Oakewood Stretch Legs", "Use the arrow keys to stretch Your legs")
            .AddFriendship("Oakewood", 1),
        new Quest("Oakewood Run", "Hold down the shift key to REALLY Stretch Your Legs")
            .AddFriendship("Oakewood", 1),
        new Quest("Oakewood Pick Up Rock", "Bring Oakewood a Rock")
            .AddFriendship("Oakewood", 1),
        new Quest("Oakewood Drop Rock", "Use z to drop the Rock")
    };

    private static bool instanceExists = false;

    private QuestCanvas questCanvas;
    private Canvas canvas;
    private Text questText;
    private GameState gameState;

    void Start()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);

        questCanvas = FindObjectOfType<QuestCanvas>();
        canvas = questCanvas.GetComponent<Canvas>();
        questText = questCanvas.GetComponentInChildren<Text>();
        gameState = FindObjectOfType<GameState>();
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
        switch (SceneManager.GetActiveScene().name)
        {
            case "Tutorial":
                CopyQuestList(QuestLoader.tutorialQuestList);
                break;
            default:
                Debug.LogError(Utils.Join("Active scene not found in quest list: ", 
                    SceneManager.GetActiveScene().name));
                break;
        }

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