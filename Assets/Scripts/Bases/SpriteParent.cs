using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteParent : MonoBehaviour
{
    public bool hasShadow = true;

    protected QuestManager questManager;

    private SpriteRenderer spriteRenderer;
    private GameObject shadow;
    private SpriteRenderer shadowRenderer;

    protected virtual void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (hasShadow)
            MakeShadow();
    }

    private void MakeShadow()
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
        if (hasShadow)
            shadowRenderer.sortingOrder = x - 1;
    }
}
