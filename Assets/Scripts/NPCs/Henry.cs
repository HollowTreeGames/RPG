using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class Henry : NPC {
    
    private System.Random random = new System.Random();

    private Quest questGreeting;
    private Quest questDankHerb;
    private Quest questDankBook;

    protected override void Start()
    {
        base.Start();
        LoadQuests();
    }

    public override void LoadQuests()
    {
        questGreeting = questManager.FindQuest("henryGreeting");
        questDankHerb = questManager.FindQuest("henryDankHerb");
        questDankBook = questManager.FindQuest("henryDankBook");
    }

    protected override void UpdateQuests()
    {
        if (questGreeting.IsAvailable())
        {
            //questGreeting.SetQuestState(QuestState.Done);
            //questDankBook.SetQuestState(QuestState.Available);
            questManager.StartQuest(questGreeting);
        }

        if (questGreeting.IsInProgress())
            return;

        if ((questDankHerb.IsUnavailable()) && (questDankBook.IsUnavailable()))
        {
            int randomNumber = random.Next(0, 2);
            Debug.Log(randomNumber);
            if (randomNumber == 1)
            {
                questDankHerb.SetQuestState(QuestState.Available);
            }
            else
            {
                questDankBook.SetQuestState(QuestState.Available);
            }
        }

        if (questDankHerb.IsDone() && questDankBook.IsUnavailable())
        {
            questDankBook.SetQuestState(QuestState.Available);
        }

        if (questDankBook.IsDone() && questDankHerb.IsUnavailable())
        {
            questDankHerb.SetQuestState(QuestState.Available);
        }
    }

    protected override bool IsQuestAvailable()
    {
        return questGreeting.IsInProgress() || questDankHerb.IsAvailable() || questDankBook.IsAvailable();
    }
}
