using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MainMenuThemeHandler : MonoBehaviour {
    [SerializeField] private float fadeDuration;
    [SerializeField] private AudioClip ambientAudioClip;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Start() {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartFading() {
        DOVirtual.Float(1.0f, 0.0f, fadeDuration, (x) => {
            _audioSource.volume = x;
        }).OnComplete(() => StartCoroutine(WaitAndPlayAmbient())); 
    }

    private IEnumerator WaitAndPlayAmbient() {
        yield return new WaitForSeconds(1.0f);
        _audioSource.clip = ambientAudioClip;
        _audioSource.Play();
        DOVirtual.Float(0.0f, 0.5f, 2.0f, (x) => {
            _audioSource.volume = x;
        });
    }

    public void FadeAudioSource(bool fade) {
        float target = fade ? 0.0f : 1.0f;
        _audioSource ??= GetComponent<AudioSource>();
        float from = _audioSource.volume <= 0 ? 0 : 1;
        DOVirtual.Float(from, target, 2.0f, (x) => {
            _audioSource.volume = x;
        });
    }
}
