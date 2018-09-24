using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;
using System;
using Yarn.Unity;

public class Item : Interactable {

    public string itemName;
    public Sprite sprite;
    public Vector2[] spawnPoints;
    public bool stickyObject;
    public bool respawnAfterPickup;

    [Header("Optional")]
    public TextAsset scriptToLoad;

    protected QuestManager questManager;
    protected InventoryManager inventoryManager;
    protected DialogueRunner dialogueRunner;
    protected GameState gameState;

    private System.Random random;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>().sprite;
        questManager = FindObjectOfType<QuestManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        gameState = FindObjectOfType<GameState>();

        random = new System.Random(this.GetHashCode() * (DateTime.Now.Millisecond + 1));

        RandomSpawn();

        if (scriptToLoad != null)
        {
            dialogueRunner.AddScript(scriptToLoad);
        }
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
                dialogueRunner.StartDialogue(itemName + " Pick Up");
                inventoryManager.SetInventory(this);
                Respawn();
            }
            else
            {
                dialogueRunner.StartDialogue(itemName + " Hands Full");
            }
        }
        else
        {
            dialogueRunner.StartDialogue(itemName + " Default");
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
