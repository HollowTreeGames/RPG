using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteParent : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnGUI()
    {
        int x = (int)(transform.position.y * -1000);
        spriteRenderer.sortingOrder = x;
    }
}
