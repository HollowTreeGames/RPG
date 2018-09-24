using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class FriendRock : Item
{
    private Quest questPickUpRock;
    private Quest questDropRock;
    private Quest questTalkToRock;

    protected override void Start()
    {
        base.Start();
        itemName = "Friend Rock";

        questPickUpRock = questManager.FindQuest("Henry Pick Up Rock");
        questDropRock = questManager.FindQuest("Henry Drop Rock");
        questTalkToRock = questManager.FindQuest("Henry Talk to Rock");
    }

    protected override bool CheckForPickup()
    {
        return questPickUpRock.IsInProgress();
    }

    private void Update()
    {
        if (questDropRock.IsInProgress() && Input.GetKeyDown("z"))
        {
            dialogueRunner.StartDialogue("Henry Drop Rock Complete");
        }
    }
}
