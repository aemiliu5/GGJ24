using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour {
    public static SoundEffectsManager instance;
    [SerializeField] private AudioClip buttonClip;
    private AudioSource _audioSource;
    public void Awake() {
        if (instance == null)
            instance = this;
        _audioSource = GetComponent<AudioSource>();
        AddButtonClickOnButtons();
    }

    public void PlayOneShot(AudioClip audioClip) {
        AudioSource.PlayClipAtPoint(audioClip, Vector3.zero, GetMaxVolume());   
    }

    public void PlayAudioSourceClip(AudioClip audioClip, bool shouldLoop) {
        _audioSource.clip = audioClip;
        _audioSource.loop = shouldLoop;
        _audioSource.Play();
    }

    public void StopAudioSource() {
        _audioSource.Stop();
        _audioSource.loop = false;
    }

    public void AddButtonClickOnButtons() {
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (var button in buttons) {
            button.onClick.AddListener(PlayButtonClip);
        }
    }

    private void PlayButtonClip() {
        AudioSource.PlayClipAtPoint(buttonClip, Vector3.zero, GetMaxVolume());
    }
    
    private float GetMaxVolume() {
        float vol = 0.0f;
        if (SaveManager.instance.HasSavedKey(SaveKeywords.MasterVolume)) {
            try { vol = (float)SaveManager.instance.GetData(SaveKeywords.MasterVolume); }
            catch (InvalidCastException) {
                double castVol = (double)SaveManager.instance.GetData(SaveKeywords.MasterVolume);
                vol = (float)castVol;
            }

            return vol;
        }
        return 1.0f;
    }
}
