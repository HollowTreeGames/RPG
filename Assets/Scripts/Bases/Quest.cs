using System.Collections.Generic;
using UnityEngine;
using Enums;

[System.Serializable]
public class Quest
{
    [SerializeField]
    string questId;
    string questName;
    [SerializeField]
    QuestState questState = QuestState.Unavailable;
    bool automaticallySetQuestAvailable = true;
    int preReqReputation = 0;
    Dictionary<string, int> preReqFriendship = new Dictionary<string, int>();
    int reputationGain = 0;
    Dictionary<string, int> friendshipGains = new Dictionary<string, int>();

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

    public Quest DoNotCheckPreReqsAutomatically()
    {
        automaticallySetQuestAvailable = false;
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
        if (preReqReputation > gameState.reputation)
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
        gameState.reputation += reputationGain;
        foreach (KeyValuePair<string, int> pair in friendshipGains)
        {
            gameState.AddFriendship(pair.Key, pair.Value);
        }
    }
}
