﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteParent : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject shadow;
    private SpriteRenderer shadowRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        makeShadow();
    }

    private void makeShadow()
    {
        // New object
        shadow = new GameObject();
        shadow.name = "Shadow";
        shadow.transform.parent = transform;
        shadow.transform.position = transform.position;

        // SpriteRenderer
        shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sortingLayerName = "BlockingLayer";
        Sprite sprite = Resources.Load<Sprite>("Shadow");
        shadowRenderer.sprite = sprite;
    }

    protected virtual void OnGUI()
    {
        int x = (int)(transform.position.y * -1000);
        spriteRenderer.sortingOrder = x;
        shadowRenderer.sortingOrder = x - 1;
    }
}
