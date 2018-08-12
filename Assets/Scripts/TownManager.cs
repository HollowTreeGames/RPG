﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

    private GameState gameState;
    private GameObject[] tents;
    private DialogueManager theDM;
    private bool tentsShown = false;

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
		if ((!gameState.dialoguePlaying) && (gameState.reputation >= 4) && (!tentsShown))
        {
            ShowTents();
            theDM.StartDialogue("The Town", level1);
        }
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
