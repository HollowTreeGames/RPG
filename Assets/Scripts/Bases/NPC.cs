using System;
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
        timeBetweenMoveCounter = random.Next(1, Math.Max(timeBetweenMove, 1));
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
        questMarker.transform.position = transform.position - new Vector3(0, -1.6f, 0);
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
        if (moveSpeed == 0)
        {
            return;
        }

        if (gameState.pause)
        {
            StopWalking(endWalkTimer: false);
            return;
        }

        if (walking)
        {
            if (MovingOutsideWalkZone())
            {
                //Debug.Log(this.name + " STAHP!");
                StopWalking();
                resetTimeBetweenMoveCounter();
            }

            timeToMoveCounter -= Time.deltaTime * 1000;

            if (timeToMoveCounter < 0f)
            {
                StopWalking();
                resetTimeBetweenMoveCounter();
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime * 1000;

            if (timeBetweenMoveCounter < 0f)
            {
                StartWalking(PickRandomDirection());
                resetTimeToMoveCounter();
            }
        }
    }

    private void resetTimeBetweenMoveCounter()
    {
        timeBetweenMoveCounter = random.Next(1000, timeBetweenMove * 1000);
    }

    private void resetTimeToMoveCounter()
    {
        timeToMoveCounter = random.Next(1000, timeToMove * 1000);
    }

    private bool MovingOutsideWalkZone()
    {
        if (walkZone == null)
        {
            return false;
        }
        return (
            (transform.position.x <= minWalkPoint.x && lastMoveDirection.x < 0) ||
            (transform.position.x >= maxWalkPoint.x && lastMoveDirection.x > 0) ||
            (transform.position.y <= minWalkPoint.y && lastMoveDirection.y < 0) ||
            (transform.position.y >= maxWalkPoint.y && lastMoveDirection.y > 0)
        );
    }

    private void PushBackIntoWalkZone()
    {
        if (walkZone == null)
        {
            return;
        }
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
        if (random.Next(2) == 0)
        {
            if (walkZone != null && transform.position.x <= minWalkPoint.x)
                return new Vector2(1, 0);
            if (walkZone != null && transform.position.x >= maxWalkPoint.x)
                return new Vector2(-1, 0);
            return new Vector2(random.Next(2) == 0 ? 1 : -1, 0);
        }
        else
        {
            if (walkZone != null && transform.position.y <= minWalkPoint.y)
                return new Vector2(0, 1);
            if (walkZone != null && transform.position.y >= maxWalkPoint.y)
                return new Vector2(0, -1);
            return new Vector2(0, random.Next(2) == 0 ? 1 : -1);
        }

        //switch (rand)
        //{
        //    case 1:
        //        x =  ? 1 : -1;
        //        y = 0;
        //        break;
        //    case 2:
        //        x =  ? -1 : 1;
        //        y = 0;
        //        break;
        //    case 3:
        //        x = 0;
        //        y = transform.position.y <= minWalkPoint.y ? 1 : -1;
        //        break;
        //    case 4:
        //        x = 0;
        //        y = transform.position.y >= maxWalkPoint.y ? -1 : 1;
        //        break;
        //    default:
        //        x = 0;
        //        y = 0;
        //        break;
        //}
        //return new Vector2(x, y);
    }

    private void StartWalking(Vector2 direction)
    {

        StartWalking(direction, this.moveSpeed);
    }

    private void StartWalking(Vector2 direction, float moveSpeed)
    {
        if (moveSpeed > 0)
        {
            walking = true;
            animator.SetBool("walking", true);
            myRigidbody.velocity = direction * moveSpeed;
            lastMoveDirection = direction;

            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
        animator.SetFloat("lastMoveX", direction.x);
        animator.SetFloat("lastMoveY", direction.y);
    }

    private void StopWalking()
    {
        StopWalking(true);
    }

    private void StopWalking(bool endWalkTimer)
    {
        if (endWalkTimer)
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

    [Yarn.Unity.YarnCommand("face")]
    public void Face(string direction)
    {
        if (direction.ToLower() == "player")
        {
            TurnToPlayer(FindObjectOfType<Player>().transform.position);
            return;
        }

        StartWalking(Utils.ParseFacing(direction), 0);
    }

    /// <summary>
    /// Moves the NPC in a given direction, at a given speed.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    [Yarn.Unity.YarnCommand("move")]
    public void Move(string direction, string speed)
    {
        float fSpeed;
        try
        {
            fSpeed = float.Parse(speed);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid move speed: {0}", speed);
            return;
        }

        Debug.Log("Starting");
        StartWalking(Utils.ParseFacing(direction), fSpeed);
    }

    [Yarn.Unity.YarnCommand("stop")]
    public void Stop()
    {
        Debug.Log("Stopping");
        StopWalking();
    }

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
