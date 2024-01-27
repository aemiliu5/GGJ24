using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int funFactor; // 3 - ecstatic, 2 - happy, 1 - neutral, 0 - disappointed - <0 dies
    public int combo;

    public float time;
    
    public static GameManager instance;

    public Score scoretext;
    public int scoreMultiplier;

    public TextMeshProUGUI funText;
    public TextMeshProUGUI startText;

    public BallSpawner ballSpawner;
    public AudioSource music;

    public bool hasStarted;


    private void Start()
    {
        instance = this;
        ballSpawner = FindObjectOfType<BallSpawner>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (hasStarted)
        {
            time += Time.deltaTime;
            startText.enabled = false;
        }


        if (funFactor < 0)
        {
            Lose();
        }

        ManageCombo();

        funText.text = ballSpawner.timeSinceLastSpawn.ToString();

        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                music.Play();
                ballSpawner.timeSinceLastSpawn = 1.6875f;
                hasStarted = true;
            }
        }
    }

    public void ManageCombo()
    {
        if (combo > 10)
        {
            funFactor++;
        }
    }

    public void AddCombo()
    {
        combo++;
    }

    public void LoseCombo()
    {
        combo = 1;
        funFactor--;
    }

    public void Lose()
    {
        Debug.Log("You lost.");
    }
}
