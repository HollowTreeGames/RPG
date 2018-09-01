using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    
    public Quest[] questList;
    public int reputation;
    public string currentScene;
    public Vector2 startPosition;
    public Utils.DictionaryStringInt friendshipDict;

    public Save(QuestManager questManager, GameState gameState)
    {
        questList = questManager.GetQuestList();
        reputation = gameState.reputation;
        currentScene = gameState.currentScene;
        startPosition = gameState.startPosition;
        friendshipDict = gameState.friendshipDict;
    }

    public void LoadData(QuestManager questManager, GameState gameState)
    {
        questManager.SetQuestList(questList);
        gameState.reputation = reputation;
        //gameState.currentScene = currentScene;
        //gameState.startPosition = startPosition;
        gameState.friendshipDict = friendshipDict;
    }

    public override string ToString()
    {
        return string.Format("Save(questList={0}, reputation={1}, friendshipArray={2}", 
            questList, reputation, friendshipDict);
    }
}

