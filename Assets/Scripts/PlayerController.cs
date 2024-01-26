using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float leftBound, rightBound;

    public Ball currentBall;
    public bool canRethrowBall;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && transform.position.x > leftBound)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        
        if (Input.GetKey(KeyCode.D) && transform.position.x < rightBound)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canRethrowBall)
        {
            currentBall.ZeroVelocity();
            currentBall.ApplyBallForce();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        canRethrowBall = true;
        currentBall = col.gameObject.GetComponent<Ball>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        canRethrowBall = false;
        currentBall = null;
    }
}
