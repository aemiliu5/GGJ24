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

    public void IncreaseScoreBy(int points)
    {
        text.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f).onComplete = delegate
        {
            text.transform.DOScale(Vector3.one, 0.1f);
        };

        score += points * GameManager.instance.totalCombo;
        //text.text = "SCORE \n" + score.ToString("00000000");
    }

    void Display() {
        DOVirtual.Int(prescore, score, 1f, (x) => {
            prescore = x;
            text.text = prescore.ToString("00000000");
        }).OnComplete(()=> { Display(); });
    }
}
