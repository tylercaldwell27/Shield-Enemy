using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour {

    Rigidbody2D rb;

    public float speed = 5;


    public bool killed;

    public int shots;
    public int jumps;
    public int health = 2;
    public float fallSpeed = 2;

    public bool onGround;
    public bool shield;
    Animator anim;
    public float jumpForce;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask isGroundLayer;
    public Vector2 pos;
    public Vector2 Enemypos;

    public player _player;

    public Rigidbody2D projectile;
    public float projectileForce;
    public Transform projectileSpawnPoint;
    public float speedBoostTime = 20.0f;
    public int nextShot = 40;
    // Use this for initialization
    void Start () {
        tag = "Enemy";

        shots = 1;
        jumps = Random.Range(1, 10);
        rb = GetComponent<Rigidbody2D>();

        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (speed <= 0 || speed > 5.0f)
        {
            speed = 0f;
        }
        anim = GetComponent<Animator>();
        // checks for groundcheck
        if (!groundCheck)
        {
            groundCheck = GameObject.Find("EnemyGroundCheck").GetComponent<Transform>();
        }
        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.1f;
        }

        // checks if the player has jump force
        if (jumpForce <= 0 || jumpForce > 20.0f)
        {
            jumpForce = 10.0f;
            Debug.LogWarning("jumpForce not set on" + name + ". Defaulting to 10");
        }

    
}
	
	// Update is called once per frame
	void Update () {

  
    Enemypos = transform.position;

        
        
       if (groundCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        }


        //if the player is on the ground
    
          //  rb.velocity = new Vector2(-speed, rb.velocity.y);
        
       
        if (isGrounded)
        {
            anim.SetBool("jump", false);
            if (jumps > 0)
            {
                shield = true;
               
                anim.SetBool("attack", false);
                rb.velocity = new Vector2(0, jumpForce);
                jumps -= 1;

               
            }
            
           else if(shots > 0 && jumps <= 0)
            {
                shield = false;
                anim.SetBool("attack", false);
                rb.velocity = new Vector2(0, 0);
              
                if (projectile && projectileSpawnPoint)
                {
                   
                    if (nextShot > 0)
                    {
                        nextShot -= 1;
                        anim.SetBool("attack", true);
                    }
                    if (nextShot <= 0)
                    {
                        
                        Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), temp.GetComponent<Collider2D>(), true);
                        temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
                        shots -= 1;
                        nextShot = 80;
                        
                    }


                }
            }
           else if (shots <= 0 && jumps <= 0)
            {
                rb.velocity = new Vector2(0, 0);
                jumps = Random.Range(1, 10);
                shots = 4;
            }

            if(health < 1)
            {
                
                Destroy(gameObject);
            }

        }
        if (!isGrounded)
        {
           anim.SetBool("jump",true);
            rb.velocity += Vector2.up* Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            
        }
        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "bullet")
        {
            if(shield == true)
            {
                Destroy(collision.gameObject);
            }
            if (shield == false)
            {
                health -= 1;
                Destroy(collision.gameObject);
            }           




        }
    }




}
