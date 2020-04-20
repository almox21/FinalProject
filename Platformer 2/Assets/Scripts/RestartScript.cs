using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class RestartScript : MonoBehaviour
{
    public Text RestartText;
    public Text scoreText;
    public Text winText;
    public Text CdtimerText;
    public Text livesText;
    private Animator myAnimator;
    private bool restart;
    private float startTime;

    
    public int lives;
    public int score;
    
    [SerializeField]
    public float movementspeed;
    private bool CdtimerFinished = false;

    [SerializeField]
    public Camera camer_main;
    public Camera camer_top;
    void Start()
    {
        startTime = Time.time + 120; //Time in seconds to count down from.
        restart = false;
        RestartText.text = "";
        lives = 3;
        SetScoreText();
        SetLivesText();
        winText.text = "";
        myAnimator = GetComponent<Animator>();
        movementspeed = 5;
        
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
        if (CdtimerFinished)      //Timer finished bool.
            return;

        float t = Time.time - startTime; //Time variable.

        float m = -t % 3600;
        string minutes = ((int)m / 60).ToString("00");
        string seconds = (-t % 60).ToString("00"); //Seconds string to one decimal place.

        CdtimerText.text = minutes + ":" + seconds; //Text object output.

        if (t >= 0)                 //Timer stop and Game Over condition.
        {
            TimerStop();
            movementspeed = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) //here is the collision magic!
    {
        
        if (collision.collider.tag == "water" && lives >= 0)
        {
            lives = 0;
            SetLivesText();

        }
        if (collision.collider.tag == "Enemy" && lives >= 0)
        {
            lives = lives - 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);

        }
       
        if (collision.collider.tag == "Coin")//you hit coin!
        {
            score = score + 1;
            SetScoreText();            
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Bigcoin")//you hit coin!
        {
            score = score + 2;
            SetScoreText();
            Destroy(collision.collider.gameObject);
        }

    }

    private void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            myAnimator.SetTrigger("dead");
            winText.text = "You lose!";
            movementspeed = 0;
            RestartText.text = "Press 'T' for Restart";
            restart = true;
            
        }
       
    }
    private void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score >= 15)
        {
            winText.text = "You win! Game created by Alejandro Revilla Young";
            RestartText.text = "Press 'T' for Restart";
            restart = true;
        }
        if (score == 14)
        {
            lives = 3;
            SetLivesText();
           

        }
    }

    public void TimerStop()
    {
        winText.text = "You lose!";
        CdtimerText.color = Color.magenta;
        CdtimerFinished = true;

        RestartText.text = "Press 'T' for Restart";
        restart = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

    }

    public void Finish()
    {
        CdtimerFinished = true;
        CdtimerText.color = Color.red;
    }
}
