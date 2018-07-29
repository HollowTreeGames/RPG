using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

    public string itemName;

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        List<string> message = new List<string>();
        if (player.holdingItem != "")
        {
            message.Add("Belfry tried to pick up the " + itemName + ", but her hands are full!");
        }
        else
        {
            message.Add("Belfry picked up a " + itemName + "!");
            player.holdingItem = "Dank Herb";
        }

        FindObjectOfType<DialogueManager>().StartDialogue("", message.ToArray());

    }
}
