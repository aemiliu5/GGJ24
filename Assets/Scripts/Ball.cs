using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballForceHeight;
    public BallType ballType;
    private int points;
    private int ballPoints;
    public bool Initialized = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BallSpawner spawner;

    private void OnEnable()
    {
        if (!Initialized)
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            spawner = FindObjectOfType<BallSpawner>();
            Initialized = true;
        }
        
        DetermineBallType();
        ApplyBallForce();
        StartCoroutine(EnableCollider());
    }

    private void Update()
    {
        if (transform.position.y < -8f) 
        {
            spawner.balls.Enqueue(this.gameObject);
            GameManager.instance.LoseCombo();
            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false); 
        }
    }

    public void ApplyBallForce()
    {
        Vector3 force = new Vector3(0, ballForceHeight, 0);
        rb.AddRelativeForce(force, ForceMode2D.Impulse);
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
                ballPoints = points;
                break;
            case BallType.ManualRicochet:
                sr.color = Color.yellow;
                ballPoints = points * 2;
                break;
            case BallType.Harmful:
                sr.color = Color.red;
                break;
            case BallType.Holdable:
                sr.color = new Color(1f, 0.7f, 0f); // orange
                ballPoints = points;
                //when you put the ball on your head or feet points go +1
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public int GetBallPoint() {
        return ballPoints;
    }
    public void SetPoints(int p) {
        points = p;
    }
    // Collider starts out disabled so it doesn't count in the score
    // We enable it later to compensate.
    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = true;
    }
}

public enum BallType
{
    AutoRicochet,
    ManualRicochet,
    Holdable,
    Harmful
}

