using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public string inventoryName = "";
    public Canvas canvas;
    public Image inventoryImage;

    private CanvasGroup canvasGroup;

    // Use this for initialization
    protected void Start()
    {
        canvasGroup = canvas.GetComponent<CanvasGroup>();
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
        inventoryImage.sprite = sprite;
        canvasGroup.alpha = 1;
        return true;
    }

    public void ClearInventory()
    {
        canvasGroup.alpha = 0;
        inventoryName = "";
        inventoryImage.sprite = null;
    }
}
