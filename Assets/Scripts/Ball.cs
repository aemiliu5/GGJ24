using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    public enum HoldableBallState { Neutral, Overhead, Feet }
    [SerializeField] private Sprite normalSprite;
    public float ballForceHeight;
    public BallType ballType;
    //------ Holdable ball configs ----------------------//
    public HoldableBallState HoldableState { get; set; }
    [SerializeField] private Sprite holdableSprite;
    public bool chosenBallState; 
    //--------------------------------------------------//
    //------ Ricochet ball configs ----------------------//
    [SerializeField] private GameObject ballOutline;
    [SerializeField] private Sprite pinAsset;
    private bool _activateRicochet;
    //--------------------------------------------------//

    private int points;
    private int ballPoints;
    public bool Initialized = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BallSpawner spawner;
    public bool shouldRotate;
    private static Transform PlayerCenterPoint => GameObject.Find("Player").GetComponent<PlayerController>().CenterPoint;

    private Vector3 _originalOutlineScale;

    private void OnEnable()
    {
        if (!Initialized)
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            spawner = FindObjectOfType<BallSpawner>();
            Initialized = true;
        }

        _originalOutlineScale = ballOutline.transform.localScale;
        Transform Transform;
        (Transform = transform).rotation = Quaternion.Euler(Vector3.zero);
        Transform.localScale = Vector3.one * 0.33f;
        sr.sprite = normalSprite;
        
        ballOutline.SetActive(false);
        chosenBallState = false;
        rb.isKinematic = false;
        _activateRicochet = false;
        
        DetermineBallType();
        ApplyBallForce();
        StartCoroutine(EnableCollider());
    }

    private void Update() {
        if (transform.position.y < -9f) { DespawnBall(); }
        
        if (ballType == BallType.ManualRicochet) {
            if(shouldRotate)
                transform.Rotate(Vector3.forward, 359.0f * Time.deltaTime);
            if (transform.position.y > 4) {
                shouldRotate = true;
                ballOutline.SetActive(true);
                _activateRicochet = true;
            }
            
            if (_activateRicochet) {
                Vector2 scale = ballOutline.transform.localScale - Vector3.one * (1.5f * Time.deltaTime);
                scale.x = Mathf.Clamp(scale.x, 1, 4);
                scale.y = Mathf.Clamp(scale.y, 1, 4);
                ballOutline.transform.localScale = scale;
            }
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

        if(transform.position.x < -8f)
            transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(2f / 2, 5f / 2));
        else if (transform.position.x > 8f)
            transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-5f / 2, -2f / 2));
        else
            transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-5f / 2, 5f / 2));
    }

    public void ZeroVelocity()
    {
       rb.velocity = Vector2.zero;
    }
    
    private void DetermineBallType()
    {
        Color[] colors = new[] { Color.red, Color.green, Color.blue, Color.yellow };
        
        switch (ballType)
        {
            case BallType.AutoRicochet:
                sr.color = colors[Random.Range(0, colors.Length)];
                ballPoints = points;
                break;
            case BallType.ManualRicochet:
                transform.localScale = Vector3.one;
                ballPoints = points * 2;
                sr.sprite = pinAsset;
                break;
            case BallType.Holdable:
                //Assign the default holdable ball state
                HoldableState = HoldableBallState.Neutral;
                sr.sprite = holdableSprite;
                transform.localScale += Vector3.one * 1.25f;
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

    public void ResetRicochet() {
        ballOutline.SetActive(false);
        ballOutline.transform.localScale = _originalOutlineScale;
        _activateRicochet = false;
    }
}

public enum BallType
{
    AutoRicochet,
    ManualRicochet,
    Holdable,
    Harmful
}

