using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefabParent;
    public Queue<GameObject> balls = new Queue<GameObject>();
    public float timeBetween;
    public float timeSinceLastSpawn;
    public int activeBalls;
    public int ballAngle;
    public int ballPoints;
    private void Start()
    {
        for(int i = 0; i < ballPrefabParent.transform.childCount; i++) 
        {
            balls.Enqueue(ballPrefabParent.transform.GetChild(i).gameObject);
        }
    }
    
    private void Update()
    {
        if (GameManager.instance.hasStarted)
        {
            SpawnBallManuallyInputs();
            SpawnBallAutomatically();
        }
    }

    private void SpawnBallAutomatically()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= timeBetween)
        {
            SpawnBall(0.2f);
            timeSinceLastSpawn = 0;
        }
    }

    private void SpawnBallManuallyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnBall(0.2f, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnBall(0.2f, 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnBall(0.2f, 2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnBall(0.2f, 3);
        }
    }
    
    private void SpawnBall(float offset, int ballType = -1)
    {
        GameObject spawned = balls.Dequeue();
        Ball ball = spawned.GetComponent<Ball>();
        ball.SetPoints(ballPoints);
        if (ballType == -1)
        {
            ball.ballType = (BallType)Random.Range(0, 1);
        }
        else
        {
            ball.ballType = (BallType)ballType;
        }

        spawned.transform.localPosition = GameObject.Find("Player").transform.position + new Vector3(0, offset, 0);
        spawned.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-ballAngle / 2, ballAngle / 2));
        spawned.SetActive(true);
    }
}
