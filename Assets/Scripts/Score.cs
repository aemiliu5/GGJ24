using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TextMeshProUGUI text;
    public int score = 0;
    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void IncreaseScoreBy(int points) {
        score += points;
        text.text = "SCORE \n" + score.ToString("00000000");
    }
}
