using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class RunningTriggers : SpriteParent
{

    private QuestManager questManager;
    private Quest quest;
    private DialogueVariableStorage dialogueVariableStorage;
    private DialogueRunner dialogueRunner;
    private bool steppedOn = false;
    private Player player;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();
        dialogueVariableStorage = FindObjectOfType<DialogueVariableStorage>();
        //quest = questManager.FindQuest("Oakewood Run");
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (quest.IsInProgress() && !steppedOn)
        //{
        //    spriteRenderer.enabled = true;
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (!quest.IsInProgress() || steppedOn)
        //    return;
        if (Input.GetButton("Fire1"))
        {
            steppedOn = true;
            dialogueVariableStorage.Increment("$oakewood_running_count");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!quest.IsInProgress() || steppedOn)
        //    return;
        if (!Input.GetButton("Fire1"))
        {
            player.Stop();
            dialogueRunner.StartDialogue("Oakewood Run Reminder");
        }
    }
}

