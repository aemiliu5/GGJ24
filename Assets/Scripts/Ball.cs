using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballForceHeight;
    public BallType ballType;
    public int points;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        DetermineBallType();
        ApplyBallForce();
    }

    private void Update()
    {
        if (transform.position.y < -8f) {
            FindObjectOfType<GameManager>().balls.Enqueue(this.gameObject);
            gameObject.SetActive(false); 
        }
    }

    public void ApplyBallForce()
    {
        Vector3 force = new Vector3(0, ballForceHeight, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ZeroVelocity()
    {
       rb.velocity = Vector2.zero;
    }
    
    private void DetermineBallType()
    {
        switch (ballType)
        {
            case BallType.AutoRicochet:
                sr.color = Color.blue;
                break;
            case BallType.ManualRicochet:
                sr.color = Color.yellow;
                break;
            case BallType.Harmful:
                sr.color = Color.red;
                break;
            case BallType.Holdable:
                sr.color = new Color(1f, 0.7f, 0f); // orange
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum BallType
{
    AutoRicochet,
    ManualRicochet,
    Harmful,
    Holdable,
}

