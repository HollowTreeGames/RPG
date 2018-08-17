using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class QuestManager : MonoBehaviour
{
    public Quest[] questList =
    {
        new Quest("Find the Dank Herb")
            .AddReputation(1)
            .AddFriendship("Henry", 1)
    };


    private static bool instanceExists = false;

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

        gameState = FindObjectOfType<GameState>();
    }

    public Quest FindQuest(string name)
    {
        foreach (Quest quest in questList)
        {
            if (quest.GetQuestName() == name)
                return quest;
        }
        throw new System.MissingMemberException("No quest found of the name: " + name);
    }

    void Update()
    {
        foreach (Quest quest in questList)
        {
            quest.CheckPreReqs(gameState);
        }
    }
}
