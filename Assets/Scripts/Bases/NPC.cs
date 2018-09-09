using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using System;

public abstract class NPC : Talkable
{
    
    protected Animator animator;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D myRigidbody;
    private System.Random random;
    protected GameState gameState;
    protected InventoryManager inventoryManager;

    #region NPC Walk
    public float moveSpeed;
    public int timeBetweenMove;
    public int timeToMove;

    private bool walking;
    private float timeBetweenMoveCounter;
    private float timeToMoveCounter;
    private Vector2 lastMoveDirection;

    public Collider2D walkZone;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    #endregion

    private GameObject questMarker;
    private SpriteRenderer questMarkerRenderer;
    private Animator questMarkerAnimator;

    protected override void Start()
    {
        base.Start();
        
        int seed = this.GetHashCode() * (DateTime.Now.Millisecond + 1);
        random = new System.Random(seed);
        timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
        myRigidbody = GetComponent<Rigidbody2D>();
        gameState = FindObjectOfType<GameState>();

        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
        }
        
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        MakeQuestMarker();
    }

    private void MakeQuestMarker()
    {
        // New object
        questMarker = new GameObject();
        questMarker.name = "Quest Marker";
        questMarker.transform.parent = transform;
        questMarker.transform.position = transform.position - new Vector3(0, -1.1f, 0);
        questMarker.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        questMarker.SetActive(false);

        // SpriteRenderer
        questMarkerRenderer = questMarker.AddComponent<SpriteRenderer>();
        questMarkerRenderer.sortingLayerName = "BlockingLayer";
        Sprite sprite = Resources.Load<Sprite>("Quest-Exclamation Mark");
        questMarkerRenderer.sprite = sprite;

        // Animator
        questMarkerAnimator = questMarker.AddComponent<Animator>();
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>("QuestMarker");
        questMarkerAnimator.runtimeAnimatorController = controller;
    }

    protected override void OnGUI()
    {
        base.OnGUI();
        int x = (int)(transform.position.y * -1000);
        questMarkerRenderer.sortingOrder = x + 1;
    }

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        TurnToPlayer(player.transform.position);
        base.Interact();
    }

    public void TurnToPlayer(Vector3 playerPosition)
    {
        // Set facing vector towards the player
        Vector3 facingVector = playerPosition - boxCollider2D.transform.position;
        animator.SetFloat("lastMoveX", facingVector.x);
        animator.SetFloat("lastMoveY", facingVector.y);
    }

    public void Update()
    {
        Walk();
        UpdateQuests();
        ShowQuestMarker();
    }

    #region Walk code
    private void Walk()
    {
        if (gameState.pause)
        {
            StopWalking(endWalkTimer: false);
            return;
        }

        if (walkZone != null)
        {
            if (!IsStillInWalkZone())
            {
                StopWalking(endWalkTimer: false);
                PushBackIntoWalkZone();
            }
        }

        if (walking)
        {
            timeToMoveCounter -= Time.deltaTime;

            if (timeToMoveCounter < 0f)
            {
                StopWalking();
                timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;

            if (timeBetweenMoveCounter < 0f)
            {
                StartWalking(PickRandomDirection());
                timeToMoveCounter = random.Next(1, timeToMove);
            }
        }
    }

    private bool IsStillInWalkZone()
    {
        return (transform.position.x > minWalkPoint.x && transform.position.x < maxWalkPoint.x &&
                transform.position.y > minWalkPoint.y && transform.position.y < maxWalkPoint.y);
    }

    private void PushBackIntoWalkZone()
    {
        if (transform.position.x <= minWalkPoint.x)
        {
            transform.position = new Vector2(minWalkPoint.x + 0.1f, transform.position.y);
        }
        if (transform.position.x >= maxWalkPoint.x)
        {
            transform.position = new Vector2(maxWalkPoint.x - 0.1f, transform.position.y);
        }
        if (transform.position.y <= minWalkPoint.y)
        {
            transform.position = new Vector2(transform.position.x, minWalkPoint.y + 0.1f);
        }
        if (transform.position.y >= maxWalkPoint.y)
        {
            transform.position = new Vector2(transform.position.x, maxWalkPoint.y - 0.1f);
        }
    }

    private Vector2 PickRandomDirection()
    {
        int x, y;
        int rand = random.Next(1, 5);
        Debug.Log(string.Format("{0} {1}", this.name, rand));

        switch (rand)
        {
            case 1:
                x = 1;
                y = 0;
                break;
            case 2:
                x = -1;
                y = 0;
                break;
            case 3:
                x = 0;
                y = 1;
                break;
            case 4:
                x = 0;
                y = -1;
                break;
            default:
                x = 0;
                y = 0;
                break;
        }
        return new Vector2(x, y);
    }

    private void StartWalking(Vector2 direction)
    {

        walking = true;
        animator.SetBool("walking", true);
        myRigidbody.velocity = direction * moveSpeed;

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetFloat("lastMoveX", direction.x);
        animator.SetFloat("lastMoveY", direction.y);
    }

    private void StopWalking()
    {
        StopWalking(true);
    }

    private void StopWalking(bool endWalkTimer)
    {
        walking = false;
        animator.SetBool("walking", false);
        myRigidbody.velocity = Vector2.zero;

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopWalking();
    }
    #endregion

    public virtual void LoadQuests()
    {
        return;
    }

    protected virtual void UpdateQuests()
    {
        return;
    }

    private void ShowQuestMarker()
    {
        if (gameState.pause)
        {
            return;
        }
        questMarker.SetActive(IsQuestAvailable());
    }

    protected virtual bool IsQuestAvailable()
    {
        return false;
    }
}
