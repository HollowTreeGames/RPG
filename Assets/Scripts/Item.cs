using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

    public string itemName;
    public Sprite sprite;

    private InventoryManager inventoryManager;
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>().sprite;
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public override void Interact()
    {
        List<string> message = new List<string>();
        if (inventoryManager.SetInventory("Dank Herb", sprite))
        {
            message.Add("Belfry picked up a " + itemName + "!");
        }
        else
        {
            message.Add("Belfry tried to pick up the " + itemName + ", but her hands are full!");
        }

        FindObjectOfType<DialogueManager>().StartDialogue("", message.ToArray());

    }
}
