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

    public string currentScene = "Main";
    public Vector2 startPosition = new Vector2(0, 0);

    // Reputation
    public int reputation = 0;

    // Friendship levels
    [SerializeField]
    public Utils.DictionaryStringInt friendshipDict = new Utils.DictionaryStringInt();

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
        Debug.Log(PrintDict());
    }

    public string PrintDict()
    {
        List<string> s = new List<string>();
        foreach (KeyValuePair<string, int> pair in friendshipDict)
        {
            s.Add(string.Format("{0}: {1}", pair.Key, pair.Value));
        }
        return "[" + string.Join(", ", s.ToArray()) + "]";
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
    }
    #endregion

    #region Save/Load Game
    public void SaveGame()
    {
        Save save = new Save(questManager, this);

        string json = JsonUtility.ToJson(save);

        string savePath = Application.persistentDataPath + "/gamesave.save";

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        File.WriteAllText(
            savePath,
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

        Debug.Log(PrintDict());
    }
    #endregion
}
