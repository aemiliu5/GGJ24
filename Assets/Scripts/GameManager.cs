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


    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        time += Time.deltaTime;

        if (funFactor < 0)
        {
            Lose();
        }

        ManageCombo();

        funText.text = combo.ToString();
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
        combo = 0;
        funFactor--;
    }

    public void Lose()
    {
        Debug.Log("You lost.");
    }
}
