using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour {

    public Vector2 offset = new Vector2(-3, 0);
    private SpriteRenderer sprRndrCaster;
    private SpriteRenderer sprRndrShadow;

    private Transform transCaster;
    private Transform transShadow;

    public Sprite shadow;

	// Use this for initialization
	void Start () {
        transCaster = transform;
        transShadow = new GameObject().transform;
        transShadow.parent = transCaster;
        transShadow.gameObject.name = "Shadow";

        sprRndrCaster = GetComponent<SpriteRenderer>();
        sprRndrShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

	}

    private void LateUpdate()
    {
        transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);

        sprRndrShadow.sprite = shadow;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
