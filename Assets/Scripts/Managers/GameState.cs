using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameState : MonoBehaviour
{
    public QuestManager questManager;

    public bool pause = false;
    // Reputation
    public int reputation = 0;

    // Friendship levels
    [SerializeField]
    public Dictionary<string, int> friendshipDict = new Dictionary<string, int>();

    #region FriendshipDict methods
    public int GetFriendship(string friend)
    {
        return friendshipDict.ContainsKey(friend) ? friendshipDict[friend] : 0;
    }

    public void AddFriendship(string friend, int gain)
    {
        if (friendshipDict.ContainsKey(friend))
        {
            friendshipDict[friend] += gain;
        }
        else
        {
            friendshipDict[friend] = gain;
        }
    }
    #endregion

    #region Singleton
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

        friendshipDict.Add("Fred", 1);
        friendshipDict.Add("Ginny", 2);
        friendshipDict.Add("Bob", 3);
    }
    #endregion

    #region Save/Load Game
    public void SaveGame()
    {
        Save save = new Save(questManager, this);

        string json = JsonUtility.ToJson(save);

        File.WriteAllText(
            Application.persistentDataPath + "/gamesave.save",
            json
        );
    }

    public void LoadGame()
    {
        string json = File.ReadAllText(
            Application.persistentDataPath + "/gamesave.save"
        );

        Save save = JsonUtility.FromJson<Save>(json);

        save.LoadData(questManager, this);
    }
    #endregion
}
