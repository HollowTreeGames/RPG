﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    
    public Quest[] questList;
    public int reputation;
    public FriendshipPair[] friendshipArray;

    public Save(QuestManager questManager, GameState gameState)
    {
        questList = questManager.GetQuestList();
        this.reputation = gameState.reputation;
        friendshipArray = DictToArray(gameState.friendshipDict);
    }

    public void LoadData(QuestManager questManager, GameState gameState)
    {
        questManager.SetQuestList(questList);
        gameState.reputation = reputation;
        gameState.friendshipDict = ArrayToDict(friendshipArray);
    }

    public override string ToString()
    {

        return string.Format("Save(questList={0}, reputation={1}, friendshipArray={2}", 
            questList, reputation, friendshipArray);
    }

    private FriendshipPair[] DictToArray(Dictionary<string, int> dict)
    {
        List<FriendshipPair> friendshipList = new List<FriendshipPair>();
        foreach (KeyValuePair<string, int> f in dict)
        {
            friendshipList.Add(new FriendshipPair(f.Key, f.Value));
        }
        return friendshipList.ToArray();
    }

    private Dictionary<string, int> ArrayToDict(FriendshipPair[] friendshipArray)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (FriendshipPair f in friendshipArray)
        {
            dict.Add(f.friend, f.level);
        }
        return dict;
    }

    [System.Serializable]
    public class FriendshipPair
    {
        public string friend;
        public int level;

        public FriendshipPair(string friend, int level)
        {
            this.friend = friend;
            this.level = level;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", friend, level);
        }
    }
}
