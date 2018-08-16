using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class Oakewood : NPC {

    private DLine[] cyclingDialogue =
    {
        new DLine("Oakewood", "Default", "Everyone's still mad at you, you know."),
        new DLine("Oakewood", "Default", "You've been known to screw things up in the past, Belfry, but this time just takes the cake."), 
        new DLine("Oakewood", "Happy", "*sigh*"),
        new DLine("Oakewood", "Default", "Why are you still talking to me?"),
        new DLine("Oakewood", "Happy", "I'm sure that big brute of a sheriff has something to say to you.")
    };
    private IEnumerator<DLine> cyclingDialogueEnumerator;
    private DLine[] friendlyDialogue =
    {
        new DLine("Oakewood", "Sad", "That was extremely unnecessary."),
        new DLine("Oakewood", "Sad", "You know that poor fox has a horrible marijuana addiction.")
    };

    // Start a new quest
    private DLine[] questBookDialogue =
    {
        new DLine("Oakewood", "Default", "As long as you're doing these ridiculous fetch quests, you might as well help me out."),
        new DLine("Oakewood", "Default", "As you well know, your last library book is overdue."),
        new DLine("Oakewood", "Happy", "How long has it been?"),
        new DLine("Oakewood", "Default", "Oh, right."),
        new DLine("Oakewood", "Sad", "FIVE YEARS."),
        new DLine("Oakewood", "Default", "It's probably a pile of ashes after your last little stunt, so I'll tell you what."),
        new DLine("Oakewood", "Default", "Find me a new book, suitable for all ages, to start the new library."),
        new DLine("Oakewood", "Default", "Then I'll consider you off the hook."),
        new DLine("Oakewood", "Sad", "For now.")
    };

    private DLine[] questBookReminderDialogue =
    {
        new DLine("Oakewood", "Default", "Have you found my new book yet?"),
        new DLine("Oakewood", "Default", "Make sure it doesn't take you five years this time.")
    };

    private DLine[] itemBookDialogue =
    {
        new DLine("Oakewood", "Default", "Hm."),
        new DLine("Oakewood", "Default", "The binding's a little worn, don't you think?"),
        new DLine("Oakewood", "Happy", "..."),
        new DLine("Oakewood", "Default", "Oh, alright. This will do.")
    };

    private DLine[] itemWrongBookDialogue =
    {
        new DLine("Oakewood", "Default", "I did say this book needs to be suitable for ALL AGES, didn't I?"),
        new DLine("Oakewood", "Default", "We don't want Piper learning to roll a blunt, now do we?"),
        new DLine("Oakewood", "Sad", "Don't answer that.")
    };

    protected override void Start()
    {
        base.Start();
        cyclingDialogueEnumerator = CycleDialogue();
    }

    private IEnumerator<DLine> CycleDialogue()
    {
        while (true) {
            foreach (DLine dLine in cyclingDialogue)
            {
                yield return dLine;
            }
        }
    }

    protected override void UpdateQuests()
    {
        if (gameState.friendshipHenry == 2 && gameState.findLibraryBook == QuestState.Unavailable)
        {
            gameState.findLibraryBook = QuestState.Available;
        }
    }

    protected override bool IsQuestAvailable()
    {
        return (gameState.findLibraryBook == QuestState.Available);
    }

    protected override DLine[] GetDialogue()
    {
        if ((gameState.findTheDankHerb == QuestState.InProgress) || (gameState.findHerbBook == QuestState.InProgress))
        {
            return friendlyDialogue;
        }

        // Find a library book
        switch (gameState.findLibraryBook)
        {
            case QuestState.Available:
                gameState.findLibraryBook = QuestState.InProgress;
                return questBookDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "Library Book")
                {
                    gameState.reputation += 1;
                    gameState.friendshipOakewood += 1;
                    inventoryManager.ClearInventory();
                    gameState.findLibraryBook = QuestState.Done;
                    return itemBookDialogue;
                }
                else if (inventoryManager.GetInventory() == "Herb Book")
                {
                    inventoryManager.ClearInventory();
                    return itemWrongBookDialogue;
                }
                else
                    return questBookReminderDialogue;

            default:
                break;
        }

        cyclingDialogueEnumerator.MoveNext();
        DLine[] dialogue = { cyclingDialogueEnumerator.Current };
        return dialogue;
    }
}
