using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour {
    public static SoundEffectsManager instance;
    [SerializeField] private AudioClip buttonClip;
    private AudioSource _audioSource;
    public void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        _audioSource = GetComponent<AudioSource>();
        AddButtonClickOnButtons();
        if (!SaveManager.instance.HasSavedKey(SaveKeywords.MasterVolume)) {
            SaveManager.instance.SaveData(SaveKeywords.MasterVolume, 0.8f);
        }
        
        float vol = 0.0f;
        try { vol = (float)SaveManager.instance.GetData(SaveKeywords.MasterVolume); }
        catch (InvalidCastException) {
            double castVol = (double)SaveManager.instance.GetData(SaveKeywords.MasterVolume);
            vol = (float)castVol;
        }

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (var source in audioSources) {
            source.volume = vol;
        }
    }

    public void PlayOneShot(AudioClip audioClip) {
        AudioSource.PlayClipAtPoint(audioClip, Vector3.zero, GetMaxVolume() + 0.2f);
        Debug.Log($"SFX VOLUME : {GetMaxVolume() + 0.2f}");
    }

    public void PlayAudioSourceClip(AudioClip audioClip, bool shouldLoop) {
        if (!_audioSource.isPlaying) {
            _audioSource.volume = GetMaxVolume() + 0.2f;
            _audioSource.clip = audioClip;
            _audioSource.loop = shouldLoop;
            _audioSource.Play();   
        }
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
        AudioSource.PlayClipAtPoint(buttonClip, Vector3.zero, GetMaxVolume() + 0.2f);
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
