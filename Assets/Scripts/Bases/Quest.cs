using System.Collections.Generic;
using UnityEngine;
using Enums;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string questId;
    [SerializeField]
    private string questName;
    [SerializeField]
    private QuestState questState = QuestState.Unavailable;
    [SerializeField]
    private bool automaticallySetQuestAvailable = false;
    [SerializeField]
    private int preReqReputation = 0;
    [SerializeField]
    private Utils.DictionaryStringInt preReqFriendship = new Utils.DictionaryStringInt();
    [SerializeField]
    private int reputationGain = 0;
    [SerializeField]
    private Utils.DictionaryStringInt friendshipGains = new Utils.DictionaryStringInt();

    // Used for serializing when saving game
    //[SerializeField]
    //private Utils.ArrayPair[] preReqFriendshipArray;
    //[SerializeField]
    //private Utils.ArrayPair[] friendshipGainsArray;

    #region Constructor
    public Quest(string id, string name)
    {
        questId = id;
        questName = name;
    }

    public Quest AddPrereqReputation(int rep)
    {
        preReqReputation = rep;
        return this;
    }

    public Quest AddPrereqFriendship(string friend, int rep)
    {
        preReqFriendship[friend] = rep;
        return this;
    }

    public Quest StartAutomatically()
    {
        automaticallySetQuestAvailable = true;
        return this;
    }

    public Quest AddReputation(int repGain)
    {
        reputationGain = repGain;
        return this;
    }

    public Quest AddFriendship(string friend, int gain)
    {
        friendshipGains[friend] = gain;
        return this;
    }

    public Quest InitialState(QuestState questState)
    {
        SetQuestState(questState);
        return this;
    }
    #endregion

    public string GetQuestId()
    {
        return questId;
    }

    public string GetQuestName()
    {
        return questName;
    }

    #region QuestState
    public QuestState GetQuestState()
    {
        return questState;
    }

    public bool CheckQuestState(QuestState questState)
    {
        return this.questState == questState;
    }

    public bool IsUnavailable()
    {
        return questState == QuestState.Unavailable;
    }

    public bool IsAvailable()
    {
        return questState == QuestState.Available;
    }

    public bool IsInProgress()
    {
        return questState == QuestState.InProgress;
    }

    public bool IsDone()
    {
        return questState == QuestState.Done;
    }

    public bool IsDisabled()
    {
        return questState == QuestState.Disabled;
    }

    public bool IsActive()
    {
        return (questState == QuestState.Available) || (questState == QuestState.InProgress);
    }

    public bool IsInactive()
    {
        return !IsActive();
    }

    public void SetUnavailable()
    {
        SetQuestState(QuestState.Unavailable);
    }

    public void SetAvailable()
    {
        SetQuestState(QuestState.Available);
    }

    public void SetInProgress()
    {
        SetQuestState(QuestState.InProgress);
    }

    public void SetDone()
    {
        SetQuestState(QuestState.Done);
    }

    public void SetDisabled()
    {
        SetQuestState(QuestState.Disabled);
    }

    public void SetQuestState(QuestState questState)
    {
        this.questState = questState;
    }
    #endregion

    public void CheckPreReqs(GameState gameState)
    {
        if (!automaticallySetQuestAvailable)
            return;
        if (questState != QuestState.Unavailable)
            return;
        if (gameState.reputation < preReqReputation)
            return;
        foreach (KeyValuePair<string, int> pair in preReqFriendship)
        {
            if (preReqFriendship[pair.Key] > gameState.GetFriendship(pair.Key))
                return;
        }
        Debug.Log("Enabling quest " + questId);
        questState = QuestState.Available;
    } 

    public void Finish(GameState gameState)
    {
        Debug.Log("Finishing quest: " + this.questId);
        SetDone();
        gameState.reputation += reputationGain;
        foreach (KeyValuePair<string, int> pair in friendshipGains)
        {
            gameState.AddFriendship(pair.Key, pair.Value);
        }
    }

    public void ConvertDictsToArrays()
    {
        //preReqFriendshipArray = Utils.DictToArray(preReqFriendship);
        //friendshipGainsArray = Utils.DictToArray(friendshipGains);
    }

    public void ConvertArraysToDicts()
    {
        //preReqFriendship = Utils.ArrayToDict(preReqFriendshipArray);
        //friendshipGains = Utils.ArrayToDict(friendshipGainsArray);
    }
}
