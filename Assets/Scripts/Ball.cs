using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour {
    public enum HoldableBallState { Neutral, Overhead, Feet }
    public float ballForceHeight;
    public BallType ballType;
    //------ Holdable ball configs ----------------------//
    public HoldableBallState HoldableState { get; set; }
    public bool chosenBallState; 
    //------ Holdable ball configs ----------------------//
    private int points;
    private int ballPoints;
    public bool Initialized = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BallSpawner spawner;

    private Transform _parent;

    private void OnEnable()
    {
        if (!Initialized)
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            spawner = FindObjectOfType<BallSpawner>();
            _parent = transform.parent;
            Initialized = true;
        }
        
        chosenBallState = false;
        rb.isKinematic = false;
        
        DetermineBallType();
        ApplyBallForce();
        StartCoroutine(EnableCollider());
    }

    private void Update() {
        if (transform.position.y < -8f) {
             DespawnBall();
        }
    }

    public void DespawnBall() {
        spawner.balls.Enqueue(gameObject);
        GameManager.instance.LoseCombo();
        GetComponent<Collider2D>().enabled = false;
        gameObject.SetActive(false);
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
                //Assign the default holdable ball state
                HoldableState = HoldableBallState.Neutral;
                sr.color = new Color(1f, 0.7f, 0f); // orange
                transform.localScale = Vector3.one * 1.25f;
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

    public void SleepRigidbody() {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public void FadeBall()
    {
        sr.DOFade(0.0f, 0.1f).OnComplete(DespawnBall);
    }
}

public enum BallType
{
    AutoRicochet,
    ManualRicochet,
    Holdable,
    Harmful
}

