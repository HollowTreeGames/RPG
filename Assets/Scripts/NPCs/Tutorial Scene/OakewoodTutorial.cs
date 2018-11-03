using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class OakewoodTutorial : NPC {

    public Quest questOakewoodGreeting;
    public Quest questOakewoodStretchLegs;
    public Quest questOakewoodRun;
    public Quest questOakewoodPickUpRock;
    public Quest questOakewoodDropRock;

    protected override void Start()
    {
        base.Start();
        LoadQuests();
        dialogueRunner.StartDialogue("Oakewood Intro");
    }

    public override void LoadQuests()
    {
        questOakewoodGreeting = questManager.FindQuest("Oakewood Greeting");
        questOakewoodStretchLegs = questManager.FindQuest("Oakewood Stretch Legs");
        questOakewoodRun = questManager.FindQuest("Oakewood Run");
    }

    protected override bool IsQuestAvailable()
    {
        return 
            questOakewoodGreeting.IsAvailable() || 
            questOakewoodStretchLegs.IsAvailable() ||
            questOakewoodRun.IsAvailable();
    }

}
