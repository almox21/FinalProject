using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public Text scoreText;
    public Text winText;
    public Text livesText;
    public Text RestartText;
    private Animator myAnimator;
    private bool restart;

    [SerializeField]
    public Camera camer_main;
    public Camera camer_top;

    [SerializeField]
    private float movementspeed;
    public float weight;

    public bool constraintActive;


    public new AudioSource audio;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioClip musicClipThree;

    public AudioClip musicClipFour;

    public AudioClip musicClipFive;

    public AudioClip musicClipSix;

    public AudioClip musicClipSeven;

    public AudioClip musicClipEight;


    public int score;
    public int lives;
    private bool facingRight;
    


    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;
   



    // Start is called before the first frame update
    void Start()
    {
        camer_main.enabled = true;
        camer_top.enabled = false;
        rd2d = GetComponent<Rigidbody2D>();
        restart = false;
        score = 0;
        lives = 3;
        winText.text = "";
        RestartText.text = "";
        SetScoreText();
        SetLivesText();
        audio = GetComponent<AudioSource>();
        facingRight = true;
        myAnimator = GetComponent<Animator>();
        jumpForce = 400;
        
    }

    // Update is called once per frame
    private void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            winText.text = "You lose!";
            RestartText.text = "Press 'T' for Restart";
            myAnimator.SetTrigger("dead");
            movementspeed = 0;
            jumpForce = 0;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            Destroy(this);
            restart = true;


        }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();

        float verMovement = Input.GetAxis("Vertical");


        rd2d.AddForce(new Vector2(hozMovement * movementspeed, verMovement * movementspeed));
        HandleMovement(hozMovement);

        Flip(hozMovement);

        HandleLayers();

        ResetValues();

    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
    }
 }
    private void LateUpdate()
    {
        HandleInput();
    }
    private void HandleMovement(float horizontal)
    {

        if (isGrounded && jump)
        {
            isGrounded = false;
            rd2d.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
            audio.PlayOneShot(musicClipSix, 1);
        }

        if (rd2d.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }

        rd2d.velocity = new Vector2(horizontal * movementspeed, rd2d.velocity.y);

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

    }


    private void OnCollisionEnter2D(Collision2D collision) //here is the collision magic!
    {
        if (collision.collider.tag == "Coin")//you hit coin!
        {
            score = score + 1;
            SetScoreText();
            audio.PlayOneShot(musicClipThree, 1);
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Bigcoin")//you hit coin!
        {
            score = score + 2;
            SetScoreText();
            audio.PlayOneShot(musicClipThree, 1);
            Destroy(collision.collider.gameObject);
        }
        // enemy hits you!
        if (collision.collider.tag == "Enemy" && lives >= 0)
        {
            lives = lives - 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);

        }
        if (collision.collider.tag == "Enemy"&& lives >= 1)
        {
            
            myAnimator.SetTrigger("damage");
            audio.PlayOneShot(musicClipFour, 1);
            movementspeed = 5;
        }

        if (collision.collider.tag == "Enemy" && lives == 0)
        {
            lives = 0;
            audio.PlayOneShot(musicClipFive, 1);
            SetLivesText();
            GameObject.Find("Player").SendMessage("Finish");
        }

        if (collision.collider.tag == "water" && lives >= 0)
        {
            lives = 0;
            audio.PlayOneShot(musicClipFive, 1);
            SetLivesText();
            GameObject.Find("Player").SendMessage("Finish");
        }

        if (collision.collider.tag == "speedpickup")
        {

            audio.PlayOneShot(musicClipEight, 1);
            movementspeed = 10;
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "slope")
        {
            myAnimator.SetBool("isSliding", true);
            GetComponent<Rigidbody2D>().gravityScale = 40f;
        }

        if (collision.collider.tag == "slopestop")
        {
            movementspeed = 0;
                GetComponent<Rigidbody2D>().gravityScale = 80f;
        }
        if (collision.collider.tag == "chest")
        {
            audio.PlayOneShot(musicClipSeven, 1);
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "slope")
        {
            myAnimator.SetBool("isSliding", false);
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
        if (collision.collider.tag == "slopestop")
        {
            myAnimator.SetBool("isSliding", false);
            movementspeed = 5;
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;

        }
    }
    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score >= 15)
        {
            GameObject.Find("Player").SendMessage("Finish");
            winText.text = "You win! Game created by Alejandro Revilla Young";
            audio.Stop();
            audio.PlayOneShot(musicClipTwo, 1);
            RestartText.text = "Press 'T' for Restart";
            restart = true;
        }

        if (score == 14) 
        {
            lives = 3;
            SetLivesText();
            transform.position = new Vector2(-10.25852f, 92.10589f);
            camer_top.enabled = true;
            camer_main.enabled = false;
            
        }


    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            jump = true;
        }

    }

    private void ResetValues()
    {
        jump = false;

    }


    private bool IsGrounded()
    {
        if (rd2d.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;

                    }

                }
            }
        }
        return false;
    }
    private void HandleLayers()

    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
   

}

     

    
