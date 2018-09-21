using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    private Quest[] questList =
    {
        new Quest("Henry Greeting", "Say Hello to that dog dude over there")
            .AddReputation(0)
            .InitialState(QuestState.Available),
        new Quest("Henry Dank Herb", "Find the Dank Herb")
            .DoNotStartAutomatically()
            .AddReputation(1)
            .AddFriendship("Henry", 1), 
        new Quest("Henry Dank Book", "Find the Dank Book")
            .DoNotStartAutomatically()
            .AddReputation(1)
            .AddFriendship("Henry", 1), 
        new Quest("Oakewood Library Book", "Find a Library Book")
            .AddPrereqFriendship("Henry", 2)
            .AddReputation(1)
            .AddFriendship("Oakewood", 1), 
        new Quest("Parsley Find CD", "Find something cool for Parsley")
            .AddPrereqReputation(3)
            .AddReputation(1)
            .AddFriendship("Parsley", 1)
    };

    #region Boring Code Stuff
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
    #endregion
}
