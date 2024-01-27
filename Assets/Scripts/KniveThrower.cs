using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KniveThrower : MonoBehaviour
{
    public GameObject kniveParent;
    public int speed;
    Queue <GameObject> knives = new Queue<GameObject>();
    GameObject currentKnive;
    bool letItFall;

    private void Start() {
        for (int i = 0; i < kniveParent.transform.childCount; i++) {
            knives.Enqueue(kniveParent.transform.GetChild(i).gameObject);
        }
    }

    private void Update() {

        if(currentKnive != null) {
            if (currentKnive.transform.position.y < -8f) {
                letItFall = false;
                knives.Enqueue(currentKnive);
                currentKnive.SetActive(false);
                currentKnive.transform.parent = kniveParent.transform;
                currentKnive.transform.localPosition = Vector3.zero;
                currentKnive = null;
            }
            else {
                if (letItFall) {
                    currentKnive.transform.parent = null;
                    currentKnive.transform.Translate(Vector3.down * Time.deltaTime * speed);
                }
            }
        }
    }


    public async void ThrowKnive() {
        GameObject knive = knives.Dequeue();
        knive.SetActive(true);
        currentKnive = knive;
        knive.GetComponent<SpriteRenderer>().DOFade(1, 2f);
        await System.Threading.Tasks.Task.Delay(4000);
        letItFall = true;
    }

}
