using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class Oakewood : NPC {

    public Quest questOakewoodGreeting;
    public Quest questOakewoodStretchLegs;
    public Quest questOakewoodRun;
    public Quest questOakewoodPickUpRock;
    public Quest questOakewoodDropRock;

    protected override void Start()
    {
        base.Start();
        LoadQuests();
    }

    public override void LoadQuests()
    {
        questOakewoodGreeting = questManager.FindQuest("Oakewood Greeting");
        questOakewoodStretchLegs = questManager.FindQuest("Oakewood Stretch Legs");
        questOakewoodRun = questManager.FindQuest("Oakewood Run");
        questOakewoodPickUpRock = questManager.FindQuest("Oakewood Pick Up Rock");
        questOakewoodDropRock = questManager.FindQuest("Oakewood Drop Rock");
    }

    protected override bool IsQuestAvailable()
    {
        return questOakewoodGreeting.IsAvailable();
    }

    protected override void UpdateQuests()
    {
        if (questOakewoodStretchLegs.IsInProgress()){

        }
    }
}
