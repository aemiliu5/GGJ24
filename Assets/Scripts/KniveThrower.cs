using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KniveThrower : MonoBehaviour {
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

    private GameManager _gameManager;

    private void Start() {
        _gameManager = GameManager.instance;
        
        for (int i = 0; i < kniveParent.transform.childCount; i++) {
            knives.Enqueue(kniveParent.transform.GetChild(i).gameObject);
        }
    }

    private void Update() {

        if (ShouldStartKnifeThrower) {
            _currentKnifeOccuranceTime += Time.deltaTime;
        
            if (_currentKnifeOccuranceTime >= knifeThrowingOccuranceTime) {
                ResetKnifeThrower();
            }
        
            //Throw knife
            if (_knifeHit > Time.time) {
                ThrowKnive();
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
                currentKnive.transform.position =
                    Vector2.Lerp(currentKnive.transform.position, pos, 10 * Time.deltaTime);
            }

            if (letItFall) {
                currentKnive.transform.Translate(Vector3.up * (Time.deltaTime * speed));
            }
        }
    }

    private void ResetKnifeThrower() {
        ShouldStartKnifeThrower = false;
        _currentKnifeOccuranceTime = 0.0f;
    }
    
    public async void ThrowKnive() {
        GameObject knive = knives.Dequeue();
        knive.SetActive(true);
        currentKnive = knive;
        _knifeShouldFollow = true;
        knive.GetComponent<SpriteRenderer>().DOFade(1, 2f);
        await System.Threading.Tasks.Task.Delay(4000);
        letItFall = true;
        _knifeShouldFollow = false;
    }

}
