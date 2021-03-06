﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static bool instanceExists = false;

    private Item itemInInventory;
    public GameObject inventory;
    private SpriteRenderer inventoryRenderer;
    private Player player;

    // Use this for initialization
    protected void Start()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(transform.gameObject);

        inventoryRenderer = inventory.GetComponent<SpriteRenderer>();
        inventory.SetActive(false);

        player = FindObjectOfType<Player>();
    }

    public string GetInventory()
    {
        if (itemInInventory == null)
            return "";
        return itemInInventory.itemName;
    }

    public bool SetInventory(Item item)
    {
        Debug.Log(this.GetHashCode());
        if (itemInInventory != null)
        {
            return false;
        }
        itemInInventory = item;
        inventoryRenderer.sprite = item.sprite;
        inventory.SetActive(true);
        player.PickupItem();
        return true;
    }

    public void ClearInventory()
    {
        inventory.SetActive(false);
        itemInInventory = null;
        inventoryRenderer.sprite = null;
        player.DropItem();
    }

    public void DropItem()
    {
        if (itemInInventory != null)
        {
            itemInInventory.gameObject.SetActive(true);
            ClearInventory();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            DropItem();
        }
    }
}
