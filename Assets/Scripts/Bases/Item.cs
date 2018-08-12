using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;

public class Item : Interactable {

    public string itemName;
    public Sprite sprite;

    protected InventoryManager inventoryManager;
    protected DialogueManager dialogueManager;
    protected GameState gameState;

    protected DLine[] pickUp = { new DLine("Belfry", "Sad", "OOPSIE WHOOPSIE WE MADE A FUCKY WUCKY") };
    protected DLine[] handsFull = { new DLine("Belfry", "Sad", "OOPSIE WHOOPSIE WE MADE A FUCKY WUCKY") };
    protected DLine[] defaultDialogue = { new DLine("Belfry", "Sad", "OOPSIE WHOOPSIE WE MADE A FUCKY WUCKY") };

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>().sprite;
        inventoryManager = FindObjectOfType<InventoryManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameState = FindObjectOfType<GameState>();
    }

    protected virtual bool CheckForPickup()
    {
        return false;
    }

    public override void Interact()
    {
        if (CheckForPickup())
        {
            if (inventoryManager.GetInventory() == "")
            {
                dialogueManager.StartDialogue(pickUp);
                inventoryManager.SetInventory(itemName, sprite);
            }
            else
            {
                dialogueManager.StartDialogue(handsFull);
            }
        }
        else
        {
            dialogueManager.StartDialogue(defaultDialogue);
        }
    }
}
