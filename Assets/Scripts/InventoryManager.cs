using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {


    public string inventoryName = "";
    public GameObject inventory;
    private SpriteRenderer inventoryRenderer;

    // Use this for initialization
    protected void Start()
    {
        inventoryRenderer = inventory.GetComponent<SpriteRenderer>();
        inventory.SetActive(false);
    }

    public string GetInventory()
    {
        return inventoryName;
    }

    public bool SetInventory(string name, Sprite sprite)
    {
        if (inventoryName != "")
        {
            return false;
        }
        inventoryName = name;
        inventoryRenderer.sprite = sprite;
        inventory.SetActive(true);
        return true;
    }

    public void ClearInventory()
    {
        inventory.SetActive(false);
        inventoryName = "";
        inventoryRenderer.sprite = null;
    }

    public void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            ClearInventory();
        }
    }
}
