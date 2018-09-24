using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTriggers : SpriteParent {

    private QuestManager questManager;
    private Quest quest;
    private DialogueVariableStorage dialogueVariableStorage;
    private bool steppedOn = false;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();
        dialogueVariableStorage = FindObjectOfType<DialogueVariableStorage>();
        quest = questManager.FindQuest("Oakewood Stretch Legs");
    }
	
	// Update is called once per frame
	void Update () {
		if (quest.IsInProgress() && !steppedOn) {
            spriteRenderer.enabled = true;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!quest.IsInProgress() || steppedOn)
            return;
            steppedOn = true;
            dialogueVariableStorage.Increment("$oakewood_walking_count");
            gameObject.SetActive(false);
    }
}
