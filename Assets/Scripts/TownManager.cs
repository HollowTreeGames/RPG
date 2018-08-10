using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

    private GameState gameState;
    private GameObject[] tents;
    private DialogueManager theDM;

    private string[] level1 =
    {
        "Congratulations! Your selfless acts have helped your fellow villagers start to rebuild. Treehollow has grown."
    };
    
	// Use this for initialization
	void Start () {
        tents = GameObject.FindGameObjectsWithTag("Tent");
        gameState = FindObjectOfType<GameState>();
        theDM = FindObjectOfType<DialogueManager>();

        HideTents();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState.reputation >= 4)
        {
            theDM.StartDialogue("The Town", level1);
            ShowTents();
        }
	}

    void HideTents()
    {
        foreach (GameObject tent in tents)
        {
            tent.SetActive(false);
        }
    }

    void ShowTents()
    {
        foreach (GameObject tent in tents)
        {
            tent.SetActive(true);
        }
    } 
}
