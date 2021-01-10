using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private GameState gameState;
    private QuestManager questManager;

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

        gameState = FindObjectOfType<GameState>();
        questManager = FindObjectOfType<QuestManager>();
    }
    #endregion

    #region Save/Load Game
    public void SaveGame()
    {
        Save save = new Save(questManager, gameState);

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

        save.LoadData(questManager, gameState);

        Debug.Log(gameState.PrintDict());

        NPC[] npcs = FindObjectsOfType<NPC>();
        foreach (NPC npc in npcs)
        {
            npc.LoadQuests();
        }
    }
    #endregion
}
