using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class King : MonoBehaviour
{
    public Sprite ecstaticSprite;
    public Sprite happySprite;
    public Sprite neutralSprite;
    public Sprite disappointedSprite;

    public float breathingAmount;
    public float breathingDuration;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        BreathInTween();
        transform.localScale = new Vector3(0.55f, 0.55f, 1f);
    }

    private void BreathInTween()
    {
        transform.DOScaleY(breathingAmount, breathingDuration).OnComplete(() => BreathOutTween());
    }

    private void BreathOutTween()
    {
        transform.DOScaleY(0.5f, breathingDuration).OnComplete(() => BreathInTween());
    }


    private void Update()
    {
        switch (GameManager.instance.funFactor)
        {
            case 0:
                sr.sprite = disappointedSprite;
                breathingAmount = 0.52f;
                breathingDuration = 2f;
                break;
            
            case 1:
                sr.sprite = neutralSprite;
                breathingAmount = 0.53f;
                breathingDuration = 1.5f;
                break;
            
            case 2:
                sr.sprite = happySprite;
                breathingAmount = 0.54f;
                breathingDuration = 1f;
                break;
            
            case 3:
                sr.sprite = ecstaticSprite;
                breathingAmount = 0.55f;
                breathingDuration = 0.3f;
                break;
        }
    }
}
