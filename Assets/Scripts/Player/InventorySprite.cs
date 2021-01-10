using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySprite : MonoBehaviour {

    public Player player;
    private SpriteRenderer spriteRenderer;
    
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	void OnGUI () {
        int x = (int)(player.transform.position.y * -1000 + 1);
        spriteRenderer.sortingOrder = x;
    }
}
