using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings:")]
    [SerializeField] private float walkSpeed = 1; //sets the players movement speed on the ground
    [Space(5)]



    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45f; //sets how hight the player can jump

    private int jumpBufferCounter = 0; //stores the jump button input
    [SerializeField] private int jumpBufferFrames; //sets the max amount of frames the jump buffer input is stored

    private float coyoteTimeCounter = 0; //stores the Grounded() bool
    [SerializeField] private float coyoteTime; //sets the max amount of frames the Grounded() bool is stored

    private int airJumpCounter = 0; //keeps track of how many times the player has jumped in the air
    [SerializeField] private int maxAirJumps; //the max no. of air jumps

    private float gravity; //stores the gravity scale at start
    [Space(5)]



    [Header("Ground Check Settings:")]
    [SerializeField] private Transform groundCheckPoint; //point at which ground check happens
    [SerializeField] private float groundCheckY = 0.2f; //how far down from ground chekc point is Grounded() checked
    [SerializeField] private float groundCheckX = 0.5f; //how far horizontally from ground chekc point to the edge of the player is
    [SerializeField] private LayerMask whatIsGround; //sets the ground layer
    [Space(5)]



    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed; //speed of the dash
    [SerializeField] private float dashTime; //amount of time spent dashing
    [SerializeField] private float dashCooldown; //amount of time between dashes
    [SerializeField] GameObject dashEffect;
    private bool canDash = true, dashed;
    [Space(5)]



    [Header("Attack Settings:")]
    //[SerializeField] private Transform SideAttackTransform; //the middle of the side attack area
    //[SerializeField] private Vector2 SideAttackArea; //how large the area of side attack is

    //[SerializeField] private Transform UpAttackTransform; //the middle of the up attack area
    //[SerializeField] private Vector2 UpAttackArea; //how large the area of side attack is

    //[SerializeField] private Transform DownAttackTransform; //the middle of the down attack area
    //[SerializeField] private Vector2 DownAttackArea; //how large the area of down attack is

    [SerializeField] private LayerMask attackableLayer; //the layer the player can attack and recoil off of

    [SerializeField] private float timeBetweenAttack;
    private float timeSinceAttack;

    [SerializeField] private float damage = 1;
    [SerializeField] private float hitForce = 10f;

    [SerializeField] private Transform attackPoligon;
    [Space(5)]

    private PlayerStateList pState;
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float health;
    //Input Variables
    private float xAxis, yAxis;
    private bool attack = false;


    //creates a singleton of the PlayerController
    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        //health = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }*/

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();

        if (pState.dashing) return;
        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
        
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-9, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(9, transform.localScale.y);
        }
    }

    private void Move()
    {
       
        if (!pState.attacking && pState.dashing == false)
        {
            rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
            anim.SetBool("Run", rb.velocity.x != 0 && Grounded());
            if (Input.GetButtonDown("Horizontal")) pState.running = true;
        }  
        if(Input.GetButtonUp("Horizontal")) pState.running = false;

    }

    void StartDash()
    {
        if (Input.GetKeyDown(KeyCode.J) && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        //pState.running = false;
        anim.SetTrigger("Dash");
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        //if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Attack()
    {
        //Collider2D attackPoligonCollider = attackPoligon.GetComponent<Collider2D>();
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            pState.attacking = true;
            pState.running = false;
            timeSinceAttack = 0;
            anim.SetTrigger("Attack");

            /*if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                Hit(SideAttackTransform, SideAttackArea);
            }
            else if (yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea);
            }
            else if (yAxis < 0 && !Grounded())
            {
                Hit(DownAttackTransform, DownAttackArea);
            }*/
            
        }
        if(timeSinceAttack > 0.8f) pState.attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.EnemyHit(damage, (transform.position - e.transform.position).normalized, hitForce);
            return;
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        List<Enemy> hitEnemies = new List<Enemy>();

        for(int i = 0; i < objectsToHit.Length; i++)
        {
            Enemy e = objectsToHit[i].GetComponent<Enemy>();
            if (e && !hitEnemies.Contains(e))
            {
                e.EnemyHit(damage, (transform.position - objectsToHit[i].transform.position).normalized, hitForce);
                hitEnemies.Add(e);
            }
        }
    
    }

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            pState.jumping = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                //pState.running = false;
                pState.jumping = true;
            }
            
        }
        if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
        {
            //pState.running = false;
            pState.jumping = true;

            airJumpCounter++;

            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpBufferFrames);
            pState.jumping = true;
            //pState.running = false;
        }

        anim.SetBool("Jumping", !Grounded());
    }


    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
}
