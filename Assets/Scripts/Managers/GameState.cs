using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class GameState : MonoBehaviour
{
    public bool pause = false;

    // Quests
    public QuestState findTheDankHerb = QuestState.Unavailable;
    public QuestState findHerbBook = QuestState.Unavailable;
    public QuestState findLibraryBook = QuestState.Unavailable;
    public QuestState findCD = QuestState.Unavailable;

    // Reputation
    public int reputation = 0;

    // Friendship levels
    public int friendshipHenry = 0;
    public int friendshipOakewood = 0;
    public int friendshipParsley = 0;




    private static bool instanceExists = false;

    void Start()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }
}
