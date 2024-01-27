using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InGameAudioMixer : MonoBehaviour {
    #region Singleton
    public static InGameAudioMixer instance;
    public void Awake() {
        if (instance == null)
            instance = this;
    }
    #endregion

    [SerializeField] private AudioSource baseAudioSource;
    [SerializeField] private AudioSource[] additionalTracks;
    [SerializeField] private float transitionTimeStep = 0.5f;

    public FunFactor FunFactor { get; set; }
    
    private float _currentFullVolume;
    private FunFactor _currentFunFactor;

    private SaveManager _saveManager;

    private void Start() {
        _saveManager = SaveManager.instance;
        Initialize();
    }

    private void Update() {
        ChangeFunFactor();
    }

    private void Initialize() {
        _currentFunFactor = 0;
        FunFactor = (FunFactor)1;
        ChangeFunFactor();
        
        baseAudioSource.Stop();
        foreach (var track in additionalTracks) { track.Stop(); }
    }

    public void EnableMusic() {
        baseAudioSource.Play();
        foreach (var track in additionalTracks) { track.Play(); }
    }

    private void ChangeFunFactor() {
        if (_currentFunFactor == FunFactor) return;
        switch (FunFactor) {
            case FunFactor.Disappointed:
                ChangeSourceVolume(additionalTracks[0], false);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Neutral:
                ChangeSourceVolume(additionalTracks[0], false);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Happy:
                ChangeSourceVolume(additionalTracks[0], true);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Ecstatic:
                ChangeSourceVolume(additionalTracks[0], true);
                ChangeSourceVolume(additionalTracks[1], true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        StartCoroutine(WaitAndSet(FunFactor));
    }

    private IEnumerator WaitAndSet(FunFactor factor) {
        yield return new WaitForSeconds(transitionTimeStep + 0.05f);
        _currentFunFactor = factor;
    }

    private void ChangeSourceVolume(AudioSource audioSource, bool fullVolume) {
        float vol = 0.0f;
        try { vol = (float)_saveManager.GetData(SaveKeywords.MasterVolume); }
        catch (InvalidCastException) {
            double castVol = (double)_saveManager.GetData(SaveKeywords.MasterVolume);
            vol = (float)castVol;
        }
        
        _currentFullVolume = vol;
        
        float targetVolume = fullVolume ? _currentFullVolume : 0;
        float originalVolume = audioSource.volume;
        DOVirtual.Float(originalVolume, targetVolume, transitionTimeStep, (x) => {
            audioSource.volume = x;
        });
    }
}
