using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KniveThrower : MonoBehaviour {
    public static KniveThrower instance;

    private void Awake() {
        instance = this;
    }

    public bool ShouldStartKnifeThrower { get; set; }
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool testing;
    [SerializeField] private float knifeThrowingOccuranceTime = 20.0f;
    public float knifeFireRate = 5;
    public GameObject kniveParent;
    public int speed;
    Queue <GameObject> knives = new Queue<GameObject>();
    GameObject currentKnive;
    bool letItFall;

    private float _knifeHit;
    private bool _knifeShouldFollow;
    private float _currentKnifeOccuranceTime;
    private float _currentXPos;
    private GameManager _gameManager;
    public AudioClip knifeSound;
    private SpriteRenderer playerSprite;

    private void Start() {
        _gameManager = GameManager.instance;
        playerSprite = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
        
        for (int i = 0; i < kniveParent.transform.childCount; i++) {
            knives.Enqueue(kniveParent.transform.GetChild(i).gameObject);
        }
    }

    private void Update() {

        if (ShouldStartKnifeThrower || testing) {
            _currentKnifeOccuranceTime += Time.deltaTime;
        
            if (_currentKnifeOccuranceTime >= knifeThrowingOccuranceTime) {
                ResetKnifeThrower();
            }
        
            //Throw knife
            if (Time.time > _knifeHit) {
               StartCoroutine(ThrowKnive());
                _knifeHit = Time.time + knifeFireRate;
            }   
        }

        if (currentKnive == null) return;

        //Reset knife
        if (currentKnive.transform.position.y < -8f) {
            letItFall = false;
            knives.Enqueue(currentKnive);
            currentKnive.SetActive(false);
            currentKnive.transform.localPosition = Vector3.zero;
            currentKnive = null;
        }
        //Handle knife behavior
        else {
            if (_knifeShouldFollow) {
                Vector2 pos = new Vector2(playerTransform.position.x, kniveParent.transform.position.y);
                currentKnive.transform.position = Vector2.Lerp(currentKnive.transform.position, pos, 10 * Time.deltaTime);
                _currentXPos = pos.x;
            }

            if (letItFall) {
                Debug.Log("SHOULD BE FALLING");
                Vector2 pos = new Vector2(_currentXPos,  currentKnive.transform.position.y);
                pos.y -= speed * Time.deltaTime;
                currentKnive.transform.position = pos;
            }
        }
    }

    private void ResetKnifeThrower() {
        ShouldStartKnifeThrower = false;
        _currentKnifeOccuranceTime = 0.0f;
    }

    public void DamagePlayer() {
        //Lose all combos & lower mood
        GameManager.instance.LoseCombo();
        GameManager.instance.LowerMood();
        playerSprite.DOColor(Color.red, 0.05f).OnComplete(() => playerSprite.DOColor(Color.white, 0.05f));
        SoundEffectsManager.instance.PlayOneShot(knifeSound);
    }
    
    public IEnumerator ThrowKnive() {
        GameObject knive = knives.Dequeue();
        knive.SetActive(true);
        currentKnive = knive;
        _knifeShouldFollow = true;
        knive.GetComponent<SpriteRenderer>().DOFade(1, 2f);
        yield return new WaitForSeconds(4f);
        knive.GetComponent<Collider2D>().enabled = true;
        letItFall = true;
        _knifeShouldFollow = false;
    }

}
