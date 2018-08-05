using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        string message;
        if (inventoryManager.SetInventory(itemName, sprite))
        {
            message = "Belfry picked up a " + itemName + "!";
        }
        else
        {
            message = "Belfry tried to pick up the " + itemName + ", but her hands are full!";
        }

        dialogueManager.StartDialogue("", message);
    }
}
