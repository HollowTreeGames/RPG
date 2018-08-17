using System.Collections.Generic;
using Enums;

public class Quest
{
    string questName;
    QuestState questState = QuestState.Unavailable;
    bool automaticallySetQuestAvailable = true;
    int preReqReputation = 0;
    Dictionary<string, int> preReqFriendship = new Dictionary<string, int>();
    int reputationGain = 0;
    Dictionary<string, int> friendshipGains = new Dictionary<string, int>();

    #region Constructor
    public Quest(string name)
    {
        questName = name;
    }

    public Quest AddPrereqReputation(int rep)
    {
        preReqReputation = rep;
        return this;
    }

    public Quest AddPrereqFriendship(string friend, int rep)
    {
        friendshipGains[friend] = rep;
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
    #endregion

    public string GetQuestName()
    {
        return questName;
    }

    public QuestState GetQuestState()
    {
        return questState;
    }

    public void SetQuestState(QuestState questState)
    {
        this.questState = questState;
    }

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
