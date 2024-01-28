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
    [SerializeField] private AudioClip loseAudioClip;

    [SerializeField] private float pneustaVolumeContributionPercentage;
    [SerializeField] private float synthVolumeContributionPercentage;

    public FunFactor FunFactor { get; set; }
    
    private float _currentFullVolume;
    private FunFactor _currentFunFactor;

    private SaveManager _saveManager;

    private void Start() {
        _saveManager = SaveManager.instance;
        Initialize();
        FunFactor = FunFactor.Disappointed;
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
                ChangeSourceVolume(additionalTracks[0], false, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], false, synthVolumeContributionPercentage);
                break;
            case FunFactor.Neutral:
                ChangeSourceVolume(additionalTracks[0], false, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], false, synthVolumeContributionPercentage);
                break;
            case FunFactor.Happy:
                ChangeSourceVolume(additionalTracks[0], true, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], false, synthVolumeContributionPercentage);
                break;
            case FunFactor.Ecstatic:
                ChangeSourceVolume(additionalTracks[0], true, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], true, synthVolumeContributionPercentage);
                break;
        }
        
        //Debug.Log(FunFactor);
        StartCoroutine(WaitAndSet(FunFactor));
    }

    public void ApplyFunFactorSettings() {
        switch (FunFactor) {
            case FunFactor.Disappointed:
                ChangeSourceVolume(additionalTracks[0], false, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], false, synthVolumeContributionPercentage);
                break;
            case FunFactor.Neutral:
                ChangeSourceVolume(additionalTracks[0], false, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], false, synthVolumeContributionPercentage);
                break;
            case FunFactor.Happy:
                ChangeSourceVolume(additionalTracks[0], true, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], false, synthVolumeContributionPercentage);
                break;
            case FunFactor.Ecstatic:
                ChangeSourceVolume(additionalTracks[0], true, pneustaVolumeContributionPercentage);
                ChangeSourceVolume(additionalTracks[1], true, synthVolumeContributionPercentage);
                break;
        }
    }

    private IEnumerator WaitAndSet(FunFactor factor) {
        yield return new WaitForSeconds(transitionTimeStep + 0.05f);
        _currentFunFactor = factor;
    }

    private void ChangeSourceVolume(AudioSource audioSource, bool fullVolume, float maxVolumeContribution = 0.0f) {
        float vol = 0.0f;
        
        _currentFullVolume = _saveManager.HasSavedKey(SaveKeywords.MasterVolume) ? GetMaxVolume() : 0.8f;

        float targetVolume = 0.0f;
        
        if (maxVolumeContribution != 0.0f) {
            targetVolume = fullVolume ? 1 - (maxVolumeContribution / _currentFullVolume) : 0;
        }
        else {
            targetVolume = fullVolume ? _currentFullVolume : 0;
        }
        
        float originalVolume = audioSource.volume;
        DOVirtual.Float(originalVolume, targetVolume, transitionTimeStep, (x) => {
            audioSource.volume = x;
        });
    }

    public void PlayLoseSound() {
        float pneustaVolume = additionalTracks[0].volume;
        float synthVolume = additionalTracks[1].volume;
        float baseVolume = baseAudioSource.volume;
        DOVirtual.Float(pneustaVolume, 0.0f, 0.5f, (x) => {
            additionalTracks[0].volume = x;
        });
        DOVirtual.Float(synthVolume, 0.0f, 0.5f, (x) => {
            additionalTracks[1].volume = x;
        });
        DOVirtual.Float(baseVolume, 0.0f, 0.5f, (x) => {
            baseAudioSource.volume = x;
        }).OnComplete(() => {
            StartCoroutine(WaitAndFadeLoseSound());
        });
    }

    private IEnumerator WaitAndFadeLoseSound() {
        yield return new WaitForSeconds(0.05f);
        baseAudioSource.clip = loseAudioClip;
        baseAudioSource.Play();
        DOVirtual.Float(0.0f, GetMaxVolume(), 0.2f, (x) => {
            baseAudioSource.volume = x;
        });
    }

    private float GetMaxVolume() {
        float vol = 0.0f;
        if (_saveManager.HasSavedKey(SaveKeywords.MasterVolume)) {
            try { vol = (float)_saveManager.GetData(SaveKeywords.MasterVolume); }
            catch (InvalidCastException) {
                double castVol = (double)_saveManager.GetData(SaveKeywords.MasterVolume);
                vol = (float)castVol;
            }
            return vol;
        }
        return 0.8f;
    }
}
