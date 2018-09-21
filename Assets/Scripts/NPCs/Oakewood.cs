using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class Oakewood : NPC {

    public Quest questLibraryBook;

    protected override void Start()
    {
        base.Start();
        LoadQuests();
    }

    public override void LoadQuests()
    {
        questLibraryBook = questManager.FindQuest("Oakewood Library Book");
    }

    protected override bool IsQuestAvailable()
    {
        return questLibraryBook.IsAvailable();
    }
}
