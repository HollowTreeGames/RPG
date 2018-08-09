using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class Player : SpriteParent
{
    public float speed;
    public float interactDistance = 1;
    public GameState gameState;
    public DialogueManager dialogueManager;

    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2D;
    public bool walking = false;

    public float lastX = 0, lastY = -1;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    void Update()
    {

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If we're not currently talking to an NPC, 
            // find if an NPC is in front of us and 
            // start talking to them.
            if (gameState.dialoguePlaying)
            {
                dialogueManager.DisplayNextSentence();
            } else
            {
                Interact();
            }
        }
    }

    void Move()
    {
        float x, y;
        walking = false;

        if (gameState.dialoguePlaying)
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= 3;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 3;
        }

        rb2d.velocity = new Vector2(speed * x * Time.deltaTime, speed * y * Time.deltaTime);
    }

    void Interact()
    {
        Vector3 position = boxCollider2D.transform.position;
        Vector2 start = new Vector2(position.x, position.y) + boxCollider2D.offset;
        Vector2 end = start + new Vector2(lastX * interactDistance, lastY * interactDistance);

//        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
//        Debug.Log(string.Format("LayerMask: {0}", layerMask));
        RaycastHit2D hit = Physics2D.Linecast(start, end);
        Debug.Log(hit.collider);
        DrawLine(start, end, Color.green);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
				interactable.Interact();
            }
        }
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.name = "Interact Line";
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.sortingLayerName = "Drawing";
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
