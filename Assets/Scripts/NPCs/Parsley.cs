using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Enums;
using MyDialogue;

public class Parsley : NPC {

    public Quest questFindCd;

    protected override void Start()
    {
        base.Start();
        LoadQuests();
    }

    public override void LoadQuests()
    {
        //questFindCd = questManager.FindQuest("Parsley Find CD");
    }

    protected override bool IsQuestAvailable()
    {
        return questFindCd.IsAvailable();
    }
}
