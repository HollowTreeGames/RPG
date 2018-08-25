using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public abstract class NPC : Talkable
{
    
    protected Animator animator;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D myRigidbody;
    private System.Random random;
    protected GameState gameState;
    protected InventoryManager inventoryManager;

    public float moveSpeed;
    private bool walking;
    public int timeBetweenMove;
    private float timeBetweenMoveCounter;
    public int timeToMove;
    private float timeToMoveCounter;
    private float chooseDirection;
    private Vector3 moveDirection;

    public Collider2D walkZone;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkZone;

    private GameObject questMarker;
    private SpriteRenderer questMarkerRenderer;
    private Animator questMarkerAnimator;

    protected override void Start()
    {
        base.Start();

        random = new System.Random(this.GetHashCode());
        timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
        myRigidbody = GetComponent<Rigidbody2D>();
        gameState = FindObjectOfType<GameState>();

        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
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

    private void Walk()
    {

        if (gameState.pause)
        {
            walking = false;
            animator.SetBool("walking", false);
            myRigidbody.velocity = Vector2.zero;
            return;
        }

        if (walking)
        {
            timeToMoveCounter -= Time.deltaTime;

            if (timeToMoveCounter < 0f)
            {
                walking = false;
                animator.SetBool("walking", false);
                myRigidbody.velocity = Vector2.zero;
                timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;

            if (timeBetweenMoveCounter < 0f)
            {
                walking = true;
                animator.SetBool("walking", true);
                timeToMoveCounter = random.Next(1, timeToMove);

                chooseDirection = Random.Range(-1, 2);
                if (chooseDirection > 0)
                {
                    float NPCY = random.Next(-1, 2);
                    myRigidbody.velocity = new Vector2(0f, NPCY * moveSpeed);
                    if (hasWalkZone && transform.position.y > maxWalkPoint.y || transform.position.y < minWalkPoint.y)
                    {
                        walking = false;
                        animator.SetBool("walking", false);
                        myRigidbody.velocity = Vector2.zero;
                        timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
                    }
                    if (NPCY == 0f)
                    {
                        walking = false;
                        animator.SetBool("walking", false);
                    }
                    animator.SetFloat("moveX", 0f);
                    animator.SetFloat("moveY", NPCY);
                    animator.SetFloat("lastMoveX", 0f);
                    animator.SetFloat("lastMoveY", NPCY);
                }
                else
                {
                    float NPCX = random.Next(-1, 2);
                    myRigidbody.velocity = new Vector2(NPCX * moveSpeed, 0f);
                    if (hasWalkZone && transform.position.x > maxWalkPoint.x || transform.position.x < minWalkPoint.x)
                    {
                        walking = false;
                        animator.SetBool("walking", false);
                        myRigidbody.velocity = Vector2.zero;
                        timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
                    }
                    if (NPCX == 0f)
                    {
                        walking = false;
                        animator.SetBool("walking", false);
                    }
                    animator.SetFloat("moveX", NPCX);
                    animator.SetFloat("moveY", 0f);
                    animator.SetFloat("lastMoveX", NPCX);
                    animator.SetFloat("lastMoveY", 0f);
                }
            }
        }
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
