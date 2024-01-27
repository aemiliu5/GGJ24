using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float leftBound, rightBound;

    public Ball currentBall;
    public bool ballInTrigger;
    
    private void Update()
    {
        // ----- Movement -----
        if (Input.GetKey(KeyCode.A) && transform.position.x > leftBound)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        
        if (Input.GetKey(KeyCode.D) && transform.position.x < rightBound)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        // ----- Handle Balls -----
        if (ballInTrigger)
        {
            switch (currentBall.ballType)
            {
                case BallType.AutoRicochet: 
                    currentBall.ZeroVelocity();
                    currentBall.ApplyBallForce();
                    break;
                
                case BallType.ManualRicochet:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        currentBall.ZeroVelocity();
                        currentBall.ApplyBallForce();
                    }
                    
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        ballInTrigger = true;
        currentBall = col.gameObject.GetComponent<Ball>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        ballInTrigger = false;
        currentBall = null;
    }
}

