using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    [SerializeField]
    Quest[] questList =
    {
        new Quest("henryGreeting", "Say Hello to that dog dude over there")
            .AddReputation(0)
            .InitialState(QuestState.Available),
        new Quest("henryDankHerb", "Find the Dank Herb")
            .DoNotCheckPreReqsAutomatically()
            .AddReputation(1)
            .AddFriendship("Henry", 1), 
        new Quest("henryDankBook", "Find the Dank Book")
            .DoNotCheckPreReqsAutomatically()
            .AddReputation(1)
            .AddFriendship("Henry", 1), 
        new Quest("oakewoodLibraryBook", "Find a Library Book")
            .AddPrereqFriendship("Henry", 2)
            .AddReputation(1)
            .AddFriendship("Oakewood", 1), 
        new Quest("parsleyFindCD", "Find something cool for Parsley")
            .AddPrereqReputation(3)
            .AddReputation(1)
            .AddFriendship("Parsley", 1)
    };

    #region Boring Code Stuff
    private static bool instanceExists = false;

    public Canvas questCanvas;
    public Text questText;

    private GameState gameState;
    private CanvasGroup canvasGroup;

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
        canvasGroup = questCanvas.GetComponent<CanvasGroup>();
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
        quest.SetQuestState(QuestState.Done);
        SetQuestText(null);
    }

    public void SetQuestText(Quest quest)
    {
        if (quest == null)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        questText.text = quest.GetQuestName();
        canvasGroup.alpha = 1f;
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
