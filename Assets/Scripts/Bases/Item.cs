using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;

public class Item : Interactable {

    public string itemName;
    public Sprite sprite;

    private InventoryManager inventoryManager;
    protected DialogueManager dialogueManager;
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>().sprite;
        inventoryManager = FindObjectOfType<InventoryManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public override void Interact()
    {
        DLine message;
        if (inventoryManager.SetInventory(itemName, sprite))
        {
            message = new DLine("Belfry", "Default", "Belfry picked up a " + itemName + "!");
        }
        else
        {
            message = new DLine("Belfry", "Default", "Belfry tried to pick up the " + itemName + ", but her hands are full!");
        }

        dialogueManager.StartDialogue(message);
    }
}
