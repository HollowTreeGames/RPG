using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class Oakewood : NPC {
    
    private System.Random random = new System.Random();

    public Sprite LibraryBook;

    private string[] cyclingDialogue =
    {
        "Everyone's still mad at you, you know.",
        "You've been known to screw things up in the past, Belfry, but this time just takes the cake.", 
        "*sigh*",
        "Why are you still talking to me?",
        "I'm sure that big brute of a sheriff has something to say to you."
    };
    private IEnumerator<string> cyclingDialogueEnumerator;
    private string[] friendlyDialogue =
    {
        "That was extremely unnecessary.",
        "You know that poor fox has a horrible marijuana addiction." 
    };

    // Start a new quest
    private string[] questBookDialogue =
    {
        "As long as you're doing these ridiculous fetch quests, you might as well help me out.",
        "As you well know, your last library book is overdue.",
        "How long has it been?",
        "Oh, right.",
        "FIVE YEARS.",
        "It's probably a pile of ashes after your last little stunt, so I'll tell you what.",
        "Find me a new book, suitable for all ages, to start the new library.",
        "Then I'll consider you off the hook.",
        "For now."
    };

    private string[] questBookReminderDialogue =
    {
        "Have you found my new book yet?",
        "Make sure it doesn't take you five years this time."
    };

    private string[] itemBookDialogue =
    {
        "Hm.",
        "The binding's a little worn, don't you think?",
        "...",
        "Oh, alright. This will do."
    };

    private string[] itemWrongBookDialogue =
    {
        "I did say this book needs to be suitable for ALL AGES, didn't I?",
        "We don't want Piper learning to roll a blunt, now do we?",
        "Don't answer that."
    };

    protected override void Start()
    {
        base.Start();
        charName = "Oakewood";
        cyclingDialogueEnumerator = CycleDialogue();
    }

    private IEnumerator<string> CycleDialogue()
    {
        while (true) {
            foreach (string sentence in cyclingDialogue)
            {
                yield return sentence;
            }
        }
    }

    protected override string[] GetDialogue()
    {
        if (gameState.friendshipHenry == 2 && gameState.findLibraryBook == QuestState.Unavailable)
        {
            gameState.findLibraryBook = QuestState.Available;
        }

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
                } else
                    return questBookReminderDialogue;

            default:
                break;
        }

        cyclingDialogueEnumerator.MoveNext();
        string[] dialogue = { cyclingDialogueEnumerator.Current };
        return dialogue;
    }
}
