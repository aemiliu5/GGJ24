using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float decreasedSpeed = 7;
    public float leftBound, rightBound;
    public PlayerData playerData;
    public Ball currentBall;
    public bool ballInTrigger;

    [SerializeField] private Transform handBallPlace;
    [SerializeField] private Transform overheadBallPlace;
    [SerializeField] private Transform feetBallPlace;
    [SerializeField] private float holdGap = 5.0f;
    private bool _holdingBall;
    private float _yPos = -5.5f;
    private float _currentHoldTime;

    private Transform _holdableBallHolder;

    private void Update() {
        float currentSpeed = currentBall != null && currentBall.HoldableState == Ball.HoldableBallState.Overhead ? decreasedSpeed : speed;
        // ----- Movement -----
        Vector3 movement = new Vector3(transform.position.x + GetInput().x * (currentSpeed * Time.deltaTime), _yPos, 0.0f);
        movement.x = Mathf.Clamp(movement.x, leftBound, rightBound);
        transform.position = movement;

        if (_holdingBall) {
            HandleHoldingBall();
            if(currentBall != null)
                currentBall.transform.position = Vector3.Lerp(currentBall.transform.position, _holdableBallHolder.position, 20 * Time.deltaTime);
        }

        // ----- Handle Balls -----
        if (!ballInTrigger) return;
        switch (currentBall.ballType)
        {
            case BallType.AutoRicochet:
                HittingBall();
                break;
                
            case BallType.ManualRicochet:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    HittingBall();
                }
                break;
            case BallType.Holdable:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    _holdingBall = true;
                    currentBall.SleepRigidbody();
                    _holdableBallHolder = handBallPlace;
                }
                break;
            case BallType.Harmful:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleHoldingBall() {
        if (_currentHoldTime >= 5) {
            //Handle exit holdable state
            RewardForHoldableState();
            ResetHoldableState();
        }
        else {
            if (Input.GetKey(KeyCode.Space)) {
                if (!currentBall.chosenBallState) {
                    if (Input.GetKeyDown(KeyCode.W)) {
                        currentBall.HoldableState = Ball.HoldableBallState.Overhead;
                        _holdableBallHolder = overheadBallPlace;
                        currentBall.chosenBallState = true;
                    }

                    if (Input.GetKeyDown(KeyCode.S)) {
                        currentBall.HoldableState = Ball.HoldableBallState.Feet;
                        _holdableBallHolder = feetBallPlace;
                        currentBall.chosenBallState = true;
                        _yPos = -5.0f;
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                //Handle leave
                RewardForHoldableState();
                ResetHoldableState();
            }
        }

        _currentHoldTime += Time.deltaTime;
        _currentHoldTime = Mathf.Clamp(_currentHoldTime, 0, holdGap);
    }

    private void RewardForHoldableState() {
        //_currentHoldTimer * base ball score -> Round to int
        switch (currentBall.HoldableState) {
            case Ball.HoldableBallState.Neutral:
                break;
            case Ball.HoldableBallState.Overhead:
                break;
            case Ball.HoldableBallState.Feet:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }        
    }

    private void ResetHoldableState() {
        currentBall.FadeBall();
        _currentHoldTime = 0.0f;
        ballInTrigger = false;
        _holdingBall = false;
        _yPos = -5.5f;
        currentBall = null;
    }

    private void HittingBall() {
        currentBall.ZeroVelocity();
        currentBall.ApplyBallForce();
        GameManager.instance.scoretext.IncreaseScoreBy(currentBall.GetBallPoint());
        GameManager.instance.AddCombo();
        ballInTrigger = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        ballInTrigger = true;
        currentBall = col.gameObject.GetComponent<Ball>();
    }

    private void OnTriggerExit2D(Collider2D col) {
        ballInTrigger = false;
        if(!_holdingBall)
            currentBall = null;
    }

    public void PlayerNaming(string name) {
        playerData.playerName = name;
    }

    private Vector2 GetInput() {
        return new Vector2() {
            x = currentBall != null && currentBall.HoldableState == Ball.HoldableBallState.Feet ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal")
        };
    }

    public void StatsToPassToPlayerData(int funRating, int score) {
        playerData.funRating = funRating;
        playerData.playerScore = score;
    }
}

