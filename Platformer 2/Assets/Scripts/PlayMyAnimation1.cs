using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMyAnimation1 : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimator.SetBool("touch1", true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            myAnimator.SetBool("touch1", false);
        }
    }
}


    

