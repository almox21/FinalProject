using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMyAnimation2 : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimator.SetBool("touch2", true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            myAnimator.SetBool("touch2", false);
        }
    }
}


    

