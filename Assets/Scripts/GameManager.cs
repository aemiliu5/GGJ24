using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;
    public int funFactor; // 3 - ecstatic, 2 - happy, 1 - neutral, 0 - disappointed - <0 dies
    public int combo;

    public GameObject ballPrefabParent;
    public Queue<GameObject> balls = new Queue<GameObject>();

    public static GameManager instance;
    
    private void Start()
    {
        for(int i = 0; i < ballPrefabParent.transform.childCount; i++) {
            balls.Enqueue(ballPrefabParent.transform.GetChild(i).gameObject);
        }
        Debug.Log(balls.Count);
        instance = this;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnBall(BallType.AutoRicochet, 0.2f);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnBall(BallType.ManualRicochet, 0.2f);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnBall(BallType.Harmful, 0.2f);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnBall(BallType.Holdable, 0.2f);
        }
    }

    private void SpawnBall(BallType ballType, float offset)
    {

        GameObject spawned = balls.Dequeue();
        spawned.SetActive(true);
        spawned.transform.localPosition = GameObject.Find("Player").transform.position + new Vector3(0, offset, 0);
        spawned.transform.localRotation = Quaternion.identity;
        spawned.GetComponent<Ball>().ballType = ballType;
    }
}
