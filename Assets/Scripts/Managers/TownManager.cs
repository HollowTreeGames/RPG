using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;

public class TownManager : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private GameState gameState;
    private GameObject[] tents;
    private bool tentsShown = false;

    private DLine[] level1 =
    {
        new DLine("The Town", "Default", "Congratulations! Your selfless acts have helped your fellow villagers start to rebuild. Treehollow has grown.")
    };

    private static bool instanceExists = false;

    // Use this for initialization
    void Start ()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(transform.gameObject);

        gameState = FindObjectOfType<GameState>();
        tents = GameObject.FindGameObjectsWithTag("Tent");

        HideTents();
	}
	
	// Update is called once per frame
	void Update () {
		//if ((!gameState.pause) && (gameState.reputation >= 4) && (!tentsShown))
  //      {
  //          ShowTents();
  //          dialogueManager.StartDialogue(level1);
  //      }
	}

    void HideTents()
    {
        ShowTents(false);
    }

    void ShowTents(bool show=true)
    {
        tentsShown = show;
        foreach (GameObject tent in tents)
        {
            tent.SetActive(show);
        }
    } 
}
