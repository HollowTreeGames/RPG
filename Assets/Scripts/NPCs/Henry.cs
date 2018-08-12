using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using MyDialogue;

public class Henry : NPC {
    
    private bool hasTalked = false;
    private System.Random random = new System.Random();

    public Sprite DankHerb;
    public Sprite HerbBook;

    // Initial dialogues
    private DLine[] initialDialogue =
    {
        new DLine("Henry", "Default", "Hi! My name is Henry! I'm the sheriff!"),
        new DLine("Belfry", "Happy", "I know who you are, Henry! We lived in Treehollow together!"),
        new DLine("Henry", "Happy", "Of course!"),
        new DLine("Henry", "Default", "But it's my duty as sheriff to greet everyone!"),
        new DLine("Henry", "Default", "Allow me to finish my introduction. Ahem."),
        new DLine("Henry", "Happy", "I like sniffing butts!"),
        new DLine("Henry", "Sad", "Nora likes it when I don't wear pants, but I don't know why!"),
        new DLine("Belfry", "Sad", "...")
    };
    private DLine[] errorDialogue =
    {
        new DLine("Henry", "Sad", "ERR-OR, I AM A ROBOT, SOMETHING HAS GONE WRONG, BEEP BOOP")
    };

    // Find The Dank Herb, natch
    private DLine[] questHerbDialogue =
    {
        new DLine("Henry", "Sad", "Nora has been really stressed by her work lately. You know what she needs?"),
        new DLine("Belfry", "Default", "A nice back massage?"),
        new DLine("Henry", "Default", "That's right! A little bit of kibbles and hits!"),
        new DLine("Belfry", "Sad", "..."),
        new DLine("Henry", "Happy", "Would you please find me some nutritious nug to help Nora take the edge off?"),
        new DLine("Belfry", "Happy", "Anything for you guys!")
    };
    private DLine[] questHerbReminderDialogue =
    {
        new DLine("Henry", "Default", "Have you found some leafs of the devil's lettuce?"),
        new DLine("Belfry", "Default", "Working on it!")
    };
    private DLine[] itemHerbDialogue =
    {
        new DLine("Henry", "Happy", "Wow, thank you! Nora will be so happy to smoke this straight killer kush!")
    };

    // Find the book, y'all
    private DLine[] questBookDialogue =
    {
        new DLine("Henry", "Happy", "So I wanted to get Nora a special book..."),
        new DLine("Henry", "Default", "You didn't hear this from me, but it's a book about..."),
        new DLine("Henry", "Sad", "WEED."),
        new DLine("Belfry", "Sad", "...ah."),
        new DLine("Henry", "Default", "Can you find one for me? She's hopeless at blazing 420 365 blaze it."),
        new DLine("Belfry", "Happy", "We can't have that, now can we? I'll look.")
    };
    private DLine[] questBookReminderDialogue =
    {
        new DLine("Henry", "Default", "Have you found that book yet?"),
        new DLine("Henry", "Default", "Keep it on the down-low."),
        new DLine("Henry", "Sad", "I'm a sheriff, you know.")
    };
    private DLine[] itemBookDialogue =
    {
        new DLine("Henry", "Happy", "Thank goodness! I was worried Nora would choke on a blunt without this.")
    };

    // You've done all the quests! Thanks.
    private DLine[] thanksDialogue =
    {
        new DLine("Henry", "Default", "Thanks again for the dank dire doobies and this mad awesome book!"),
        new DLine("Henry", "Sad", "Nora asked me to stop saying dank, but it's too much fun!"),
        new DLine("Henry", "Happy", "Dank dank dank dank dank dank dank dank dank dank dank " +
                                        "dank dank dank dank dank dank dank dank dank dank dank " +
                                        "dank dank dank dank dank dank dank dank dank dank dank " +
                                        "dank dank dank dank dank dank dank dank dank dank dank " +
                                        "dank dank dank dank dank dank dank dank dank dank dank!")
    };

    protected override void UpdateQuests()
    {
        if (!hasTalked)
            return;

        if ((gameState.findTheDankHerb == QuestState.Unavailable) && (gameState.findHerbBook == QuestState.Unavailable))
        {
            int randomNumber = random.Next(0, 2);
            Debug.Log(randomNumber);
            if (randomNumber == 1)
            {
                gameState.findTheDankHerb = QuestState.Available;
            }
            else
            {
                gameState.findHerbBook = QuestState.Available;
            }
        }

        if ((gameState.findTheDankHerb == QuestState.Done) && (gameState.findHerbBook == QuestState.Unavailable))
        {
            gameState.findHerbBook = QuestState.Available;
        }

        if ((gameState.findTheDankHerb == QuestState.Unavailable) && (gameState.findHerbBook == QuestState.Done))
        {
            gameState.findTheDankHerb = QuestState.Available;
        }
    }

    protected override bool IsQuestAvailable()
    {
        return ((gameState.findTheDankHerb == QuestState.Available) ||
                (gameState.findHerbBook == QuestState.Available));
    }

    protected override DLine[] GetDialogue()
    {
        if (!hasTalked)
        {
            hasTalked = true;
            return initialDialogue;
        }

        // Find dat dank herb
        switch (gameState.findTheDankHerb)
        {
            case QuestState.Available:
                gameState.findTheDankHerb = QuestState.InProgress;
                return questHerbDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "Dank Herb")
                {
                    gameState.reputation += 1;
                    gameState.friendshipHenry += 1;
                    inventoryManager.ClearInventory();
                    gameState.findTheDankHerb = QuestState.Done;
                    return itemHerbDialogue;
                }
                else
                    return questHerbReminderDialogue;
            default:
                break;
        }

        // Get you some herb book, son
        switch (gameState.findHerbBook)
        {
            case QuestState.Available:
                gameState.findHerbBook = QuestState.InProgress;
                return questBookDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "Herb Book")
                {
                    gameState.reputation += 1;
                    gameState.friendshipHenry += 1;
                    inventoryManager.ClearInventory();
                    gameState.findHerbBook = QuestState.Done;
                    return itemBookDialogue;
                }
                else
                    return questBookReminderDialogue;
            default:
                break;
        }

        return thanksDialogue;
    }
}
