using DG.Tweening;
using UnityEngine;

public class MainMenuThemeHandler : MonoBehaviour {
    [SerializeField] private float fadeDuration;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Start() {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartFading() {
        DOVirtual.Float(1.0f, 0.0f, fadeDuration, (x) => {
            _audioSource.volume = x;
        }).OnComplete(() => Destroy(gameObject)); 
    }
}
