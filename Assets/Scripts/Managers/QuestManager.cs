﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class QuestManager : MonoBehaviour
{
    public Quest[] questList =
    {
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

    public Quest FindQuest(string id)
    {
        foreach (Quest quest in questList)
        {
            if (quest.GetQuestId() == id)
                return quest;
        }
        throw new System.MissingMemberException("No quest found with the id: " + id);
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
}