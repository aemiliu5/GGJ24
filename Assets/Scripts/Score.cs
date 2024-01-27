using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Score : MonoBehaviour
{
    TextMeshProUGUI text;
    public int score = 0;
    int prescore;
    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
        Display();
    }

    public void IncreaseScoreBy(int points) {
        score += points;
        //text.text = "SCORE \n" + score.ToString("00000000");
    }

    void Display() {
        DOVirtual.Int(prescore, score, 0.2f, (x) => {
            prescore = x;
            text.text = "SCORE \n" + prescore.ToString("00000000");
        }).OnComplete(()=> { Display(); });
    }
}
