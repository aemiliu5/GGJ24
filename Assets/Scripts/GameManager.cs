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
    private int loseCombo = 0;
    public float time;

    public PlayerController player;
    public BallSpawner ballSpawner;
    public GameObject namingPanel;
    public GameObject leaderboard;

    public static GameManager instance;

    public Score scoretext;
    public int scoreMultiplier;

    public TextMeshProUGUI funText;
    public TextMeshProUGUI startText;

    public AudioSource music;
    public bool hasStarted;
    private void Start()
    {
        instance = this;
    }

    public void BeginPlay() {
        player.enabled = true;
        namingPanel.SetActive(false);
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && player.enabled)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (hasStarted) {
            time += Time.deltaTime;
            startText.enabled = false;
        }

        if (funFactor < 0)
        {
            Lose();
        }

        ManageCombo();

        funText.text = ballSpawner.timeSinceLastSpawn.ToString();

        if (!hasStarted) {
            if (Input.GetKeyDown(KeyCode.Space) && player.enabled) {
                music.Play();
                ballSpawner.timeSinceLastSpawn = 1.6875f;
                ballSpawner.enabled = true;
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
        loseCombo = 0;
    }

    public void LoseCombo()
    {
        combo = 1;
        loseCombo++;
        if(loseCombo >= 10) { funFactor--; }
    }

    public void Lose()
    {
        player.enabled = false;
        ballSpawner.enabled = false;
        leaderboard.SetActive(true);
    }
}
