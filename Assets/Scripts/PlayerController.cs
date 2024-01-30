using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float decreasedSpeed = 7;
    public float leftBound, rightBound;
    public PlayerData playerData;
    public Ball currentBall;
    [FormerlySerializedAs("holdingBall")] public Ball holdingBallObject;
    [Header("Ball Audio Clips")]
    [SerializeField] private AudioClip spinningHoldingBallClip;
    [SerializeField] private AudioClip hitAutoRicochetBallClip;
    [SerializeField] private AudioClip hitManualRicochetBallClip;
    [SerializeField] private AudioClip knifeContactClip;
    
    public bool ballInTrigger;

    [Header("Sprites")] 
    public float animationTimer;
    public float animationTimerEnd;
    public bool playingNeutralAnim;
    public Sprite juggleNeutralSprite1;
    public Sprite juggleNeutralSprite2;
    public Sprite juggleNeutralSprite3;
    public Sprite juggleHolding;
    public Sprite juggleOverhead;
    public Sprite juggleUnderfeet1;
    public Sprite juggleUnderfeet2;
    public Sprite juggleUnderfeet3;
    public Sprite juggleUnderfeet4;
    

    [SerializeField] private Transform triggerPoint;
    [SerializeField] private Transform handBallPlace;
    [SerializeField] private Transform overheadBallPlace;
    [SerializeField] private Transform feetBallPlace;
    [SerializeField] private float holdGap = 5.0f;

    public Transform CenterPoint => triggerPoint;
    
    private bool _holdingBall;
    private float _yPos = -5.5f;
    private float _currentHoldTime;

    private Transform _holdableBallHolder;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float currentSpeed =
            holdingBallObject != null && holdingBallObject.HoldableState == Ball.HoldableBallState.Overhead
                ? decreasedSpeed
                : speed;
        // ----- Movement -----
        Vector3 movement = new Vector3(transform.position.x + GetInput().x * (currentSpeed * Time.deltaTime), _yPos,
            0.0f);
        movement.x = Mathf.Clamp(movement.x, leftBound, rightBound);
        transform.position = movement;

        if (_holdingBall && holdingBallObject.ballType == BallType.Holdable)
        {
            HandleHoldingBall();
            if (holdingBallObject != null)
                holdingBallObject.transform.position = Vector3.Lerp(holdingBallObject.transform.position,
                    _holdableBallHolder.position, 50 * Time.deltaTime);
        }

        animationTimer += Time.deltaTime;
        HandleCurrentSprite();
        
        // ----- Handle Balls -----
        if (!ballInTrigger) return;
        switch (currentBall.ballType)
        {
            case BallType.AutoRicochet:
                HittingBall();
                SoundEffectsManager.instance.PlayOneShot(hitAutoRicochetBallClip);
                break;
            case BallType.ManualRicochet:
                if (Input.GetKeyDown(KeyCode.J)) {
                    HittingBall();
                    SoundEffectsManager.instance.PlayOneShot(hitManualRicochetBallClip);
                    currentBall.ResetRicochet();
                }

                break;
            case BallType.Holdable:
                if (_holdingBall) return;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _holdingBall = true;
                    holdingBallObject.SleepRigidbody();
                    _holdableBallHolder = handBallPlace;
                }

                break;
        }
    }



    private void HandleHoldingBall() {
        if (_currentHoldTime >= 5) {
            //Handle exit holdable state
            RewardForHoldableState(true);
            ResetHoldableState();
        }
        else {
            if (Input.GetKey(KeyCode.Space)) {
                if (!holdingBallObject.chosenBallState) {
                    SoundEffectsManager.instance.PlayAudioSourceClip(spinningHoldingBallClip, true);
                    if (Input.GetKeyDown(KeyCode.W)) {
                        holdingBallObject.HoldableState = Ball.HoldableBallState.Overhead;
                        _holdableBallHolder = overheadBallPlace;
                        holdingBallObject.chosenBallState = true;
                    }

                    if (Input.GetKeyDown(KeyCode.S)) {
                        holdingBallObject.HoldableState = Ball.HoldableBallState.Feet;
                        _holdableBallHolder = feetBallPlace;
                        holdingBallObject.chosenBallState = true;
                        _yPos = -5.0f;
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space)) {
                //Handle leave
                RewardForHoldableState(false);
                ResetHoldableState();
            }
        }

        _currentHoldTime += Time.deltaTime;
        _currentHoldTime = Mathf.Clamp(_currentHoldTime, 0, holdGap);
    }

    private void RewardForHoldableState(bool fullHold) {
        //_currentHoldTimer * base ball score -> Round to int
        int pointsToGive = 8;
        int finalPoints = 0;
        switch (holdingBallObject.HoldableState) {
            case Ball.HoldableBallState.Neutral:
                 finalPoints = fullHold ?  Mathf.RoundToInt((pointsToGive * 5) + ((pointsToGive * 5) / 3))  : Mathf.RoundToInt(pointsToGive * _currentHoldTime);
                GameManager.instance.scoretext.IncreaseScoreBy(finalPoints);
                break;
            case Ball.HoldableBallState.Overhead:
                pointsToGive += 1;
                finalPoints = fullHold ?  Mathf.RoundToInt((pointsToGive * 5) + ((pointsToGive * 5) / 3))  : Mathf.RoundToInt(pointsToGive * _currentHoldTime);
                GameManager.instance.scoretext.IncreaseScoreBy(finalPoints);
                break;
            case Ball.HoldableBallState.Feet:
                pointsToGive += 1;
                finalPoints = fullHold ?  Mathf.RoundToInt((pointsToGive * 5) + ((pointsToGive * 5) / 3))  : Mathf.RoundToInt(pointsToGive * _currentHoldTime);
                GameManager.instance.scoretext.IncreaseScoreBy(finalPoints);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }        
    }

    private void ResetHoldableState() {
        holdingBallObject.FadeBall();
        SoundEffectsManager.instance.StopAudioSource();
        _currentHoldTime = 0.0f;
        ballInTrigger = false;
        _holdingBall = false;
        holdingBallObject = null;
        _yPos = -5.5f;
    }

    private void HittingBall() {
        currentBall.ZeroVelocity();
        currentBall.ApplyBallForce();
        GameManager.instance.scoretext.IncreaseScoreBy(currentBall.GetBallPoint());
        GameManager.instance.AddCombo();
        ballInTrigger = false;

        if (DOTween.IsTweening(transform))
        {
            transform.DOKill(transform);
        }
        
        transform.DOPunchScale(new Vector3(0, 0.5f, 0), 0.2f).OnComplete(() => transform.localScale = new Vector3(1,2,1));
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Ball>()) {

            currentBall = col.gameObject.GetComponent<Ball>();

            if (currentBall.ballType == BallType.ManualRicochet) {
                currentBall.shouldRotate = false;
                currentBall.transform.rotation = Quaternion.Euler(Vector3.zero);
                currentBall.GetComponent<Rigidbody2D>().totalTorque = 0;
            }

            ballInTrigger = true;
            
            if (_holdingBall) return;
            if (currentBall.ballType == BallType.Holdable)
                holdingBallObject = col.gameObject.GetComponent<Ball>();
        }
        else {
            SoundEffectsManager.instance.PlayOneShot(knifeContactClip);
            KniveThrower.instance.DamagePlayer();
            col.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.GetComponent<Ball>()) return;
        ballInTrigger = false;
        if(!_holdingBall)
            currentBall = null;
    }

    public void PlayerNaming(string name) {
        playerData.playerName = name;
    }

    private Vector2 GetInput() {
        return new Vector2() {
            x = holdingBallObject != null && holdingBallObject.HoldableState == Ball.HoldableBallState.Feet ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal")
        };
    }

    public void StatsToPassToPlayerData(int funRating, int score) {
        playerData.funRating = funRating;
        playerData.playerScore = score;
    }

    private void HandleCurrentSprite()
    {
        if (_holdingBall)
        {
            playingNeutralAnim = false;
            switch (holdingBallObject.HoldableState)
            {
                case Ball.HoldableBallState.Neutral:
                    sr.sprite = juggleHolding;
                    break;
                case Ball.HoldableBallState.Overhead:
                    sr.sprite = juggleOverhead;
                    break;
                case Ball.HoldableBallState.Feet:
                    if (animationTimer > animationTimerEnd)
                    {
                        if (sr.sprite == juggleUnderfeet1)
                            sr.sprite = juggleUnderfeet2;
                        else if (sr.sprite == juggleUnderfeet2)
                            sr.sprite = juggleUnderfeet3;
                        else if (sr.sprite == juggleUnderfeet3)
                            sr.sprite = juggleUnderfeet4;
                        else
                            sr.sprite = juggleUnderfeet1;

                        animationTimer = 0;
                    }
                    
                    break;
            }
        }
        else
        {
            if (!playingNeutralAnim && GameManager.instance.hasStarted)
            {
                StartCoroutine(PlayNormalJugglingAnimation());
            }
        }
    }

    private IEnumerator PlayNormalJugglingAnimation()
    {
        playingNeutralAnim = true;
        sr.sprite = juggleNeutralSprite1;
        yield return new WaitForSeconds(0.2f);
        sr.sprite = juggleNeutralSprite2;
        yield return new WaitForSeconds(0.2f);
        sr.sprite = juggleNeutralSprite3;
        yield return new WaitForSeconds(0.2f);
        sr.sprite = juggleNeutralSprite2;
        yield return new WaitForSeconds(0.2f);
        
        if (playingNeutralAnim)
        {
            StartCoroutine(PlayNormalJugglingAnimation());
        }
    }
}

