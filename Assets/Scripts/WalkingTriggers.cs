using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTriggers : MonoBehaviour {

    private QuestManager questManager;
    private Quest quest;
    private bool steppedOn;

	// Use this for initialization
	void Start () {
        questManager = FindObjectOfType<QuestManager>();
        quest = questManager.FindQuest("Oakewood Stretch Legs");
	}
	
	// Update is called once per frame
	void Update () {
		if (quest.IsInProgress() && !steppedOn) {
            gameObject.SetActive(true);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        steppedOn = true;
        gameObject.SetActive(false);
    }
}
