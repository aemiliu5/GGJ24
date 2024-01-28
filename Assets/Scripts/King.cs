using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class King : MonoBehaviour
{
    public Sprite ecstaticSpriteLeft, ecstaticSpriteRight;
    public Sprite happySpriteLeft, happySpriteRight;
    public Sprite neutralSprite;
    public Sprite disappointedSprite;

    public float breathingAmount;
    public float breathingDuration;
    public float spriteTimer;
    public float spriteTimerEnd;
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
        spriteTimer += Time.deltaTime;
        
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
                if (spriteTimer > spriteTimerEnd)
                {
                    if (sr.sprite == happySpriteLeft)
                        sr.sprite = happySpriteRight;
                    else
                        sr.sprite = happySpriteLeft;

                    spriteTimer = 0;
                }
                
                
                breathingAmount = 0.54f;
                breathingDuration = 1f;
                break;
            
            case >3:
                if (spriteTimer > spriteTimerEnd)
                {
                    if (sr.sprite == ecstaticSpriteLeft)
                        sr.sprite = ecstaticSpriteRight;
                    else
                        sr.sprite = ecstaticSpriteLeft;
                    
                    spriteTimer = 0;
                }

                breathingAmount = 0.55f;
                breathingDuration = 0.3f;
                break;
        }
    }
}
