using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballForceHeight;
    
    private void Start()
    {
        ApplyBallForce();
    }

    public void ApplyBallForce()
    {
        Vector3 force = new Vector3(0, ballForceHeight, 0);
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    public void ZeroVelocity()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
