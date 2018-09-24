using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class HenryTutorial : NPC
{
    public Quest questHenryPickUpRock;
    public Quest questHenryDropRock;
    public Quest questHenryTalkToRock;

    protected override void Start()
    {
        base.Start();
        LoadQuests();
    }

    public override void LoadQuests()
    {
        questHenryPickUpRock = questManager.FindQuest("Henry Pick Up Rock");
        questHenryDropRock = questManager.FindQuest("Henry Drop Rock");
        questHenryTalkToRock = questManager.FindQuest("Henry Talk to Rock");
    }

    protected override bool IsQuestAvailable()
    {
        return 
            questHenryPickUpRock.IsAvailable() || 
            questHenryDropRock.IsAvailable() ||
            questHenryTalkToRock.IsAvailable();
    }
}
