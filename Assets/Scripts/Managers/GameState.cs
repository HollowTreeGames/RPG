using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

[System.Serializable]
public class GameState : MonoBehaviour
{
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
    }
    #endregion
}
