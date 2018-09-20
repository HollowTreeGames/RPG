﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using Yarn.Unity;

public class Player : SpriteParent
{
    private static bool instanceExists = false;

    public float walkSpeed;
    public float runSpeed;
    public float interactDistance = 1;
    public GameState gameState;
    public DialogueRunner dialogueRunner;

    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2D;
    public bool walking = false;

    public float lastX = 0, lastY = -1;

    // Use this for initialization
    protected override void Start()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(transform.gameObject);

        base.Start();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        gameState = FindObjectOfType<GameState>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
    
    // Update is called once per frame
    void Update()
    {

        Move();

        if (Input.GetButtonDown("Jump"))
        {
            // If we're not currently talking to an NPC, 
            // find if an NPC is in front of us and 
            // start talking to them.
            if (!dialogueRunner.isDialogueRunning)
            { 
                Interact();
            }
        }
    }

    void Move()
    {
        float x, y;
        walking = false;

        if (dialogueRunner.isDialogueRunning)
        {
            x = 0;
            y = 0;
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if ((x > 0.5f) || (x < -0.5f))
            {
                lastX = Mathf.Round(x);
                lastY = 0;
                walking = true;
            }

            if ((y > 0.5f) || (y < -0.5f))
            {
                lastX = 0;
                lastY = Mathf.Round(y);
                walking = true;
            }
        }
        animator.SetBool("walking", walking);
        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
        animator.SetFloat("lastMoveX", lastX);
        animator.SetFloat("lastMoveY", lastY);

        float moveSpeed = Input.GetButton("Fire1") ? runSpeed : walkSpeed;

        rb2d.velocity = new Vector2(moveSpeed * x * Time.deltaTime, moveSpeed * y * Time.deltaTime);
    }

    void Interact()
    {
        Vector3 position = boxCollider2D.transform.position;
        Vector2 start = new Vector2(position.x, position.y) + boxCollider2D.offset;
        Vector2 direction = new Vector2(lastX, lastY);

        //        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        //        Debug.Log(string.Format("LayerMask: {0}", layerMask));
        RaycastHit2D hit = Physics2D.BoxCast(start, boxCollider2D.size, 0, direction, interactDistance);
        //RaycastHit2D hit = Physics2D.Linecast(start, end);
        Debug.Log(hit.collider);
        //Debug.DrawLine(start, end, Color.green, 0.5);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
				interactable.Interact();
            }
        }
    }
}
