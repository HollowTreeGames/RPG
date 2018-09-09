using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;
using System;

public class Item : Interactable {

    public string itemName;
    public Sprite sprite;
    public Vector2[] spawnPoints;
    public bool stickyObject;
    public bool respawnAfterPickup;

    protected InventoryManager inventoryManager;
    protected DialogueManager dialogueManager;
    protected GameState gameState;

    protected DLine[] pickUp = { new DLine("Belfry", "Sad", "OOPSIE WHOOPSIE WE MADE A FUCKY WUCKY") };
    protected DLine[] handsFull = { new DLine("Belfry", "Sad", "OOPSIE WHOOPSIE WE MADE A FUCKY WUCKY") };
    protected DLine[] defaultDialogue = { new DLine("Belfry", "Sad", "OOPSIE WHOOPSIE WE MADE A FUCKY WUCKY") };

    private System.Random random;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>().sprite;
        inventoryManager = FindObjectOfType<InventoryManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameState = FindObjectOfType<GameState>();

        random = new System.Random(this.GetHashCode() * (DateTime.Now.Millisecond + 1));

        RandomSpawn();
    }

    private void RandomSpawn()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Vector2 spawn = spawnPoints[random.Next(0, spawnPoints.Length)];
            transform.localPosition = spawn;
        }
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
                inventoryManager.SetInventory(this);
                Respawn();
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

    private void Respawn()
    {
        if (stickyObject)
            return;

        if (respawnAfterPickup)
        {
            RandomSpawn();
            return;
        }

        gameObject.SetActive(false);
    }
}
