using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour {
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullScreenToggle;
    private AudioSource[] _audioSources;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    
    private bool _fullScreen;
    private int _currentResolutionIndex = 0;
    private SaveManager _saveManager;

    private void Start() {
        _saveManager = SaveManager.instance;
        _audioSources = FindObjectsOfType<AudioSource>();
        InitializeResolutions();
        LoadOptions();
        gameObject.SetActive(false);
    }

    private void LoadOptions() {
        if (_saveManager.HasSavedKey(SaveKeywords.MasterVolume)) {
            float vol = 0.0f;
            try { vol = (float)_saveManager.GetData(SaveKeywords.MasterVolume); }
            catch (InvalidCastException) {
                double castVol = (double)_saveManager.GetData(SaveKeywords.MasterVolume);
                vol = (float)castVol;
            }
            SetVolume(vol);
            volumeSlider.value = vol;
        }

        if (_saveManager.HasSavedKey(SaveKeywords.ResolutionIndexKey)) {
            int resIndex = 0;
            try {
               resIndex = ((Int32)_saveManager.GetData(SaveKeywords.ResolutionIndexKey));
            }
            catch (InvalidCastException ) {
                Int64 cast = Convert.ToInt64(_saveManager.GetData(SaveKeywords.ResolutionIndexKey));
                resIndex = (int)cast;
            }
            SetCurrentRes(resIndex);
            dropdown.value = resIndex;
        }
        if (_saveManager.HasSavedKey(SaveKeywords.IsFullScreenKey)) {
            bool fullScreen = (bool)_saveManager.GetData(SaveKeywords.IsFullScreenKey);
            SetFullScreen((bool)_saveManager.GetData(SaveKeywords.IsFullScreenKey));
            fullScreenToggle.isOn = fullScreen;
        }
    }

    public void SetVolume(float volume) {
        foreach (var source in _audioSources) { source.volume = volume; }
        _saveManager.SaveData(SaveKeywords.MasterVolume, volume);
    }

    private void InitializeResolutions() {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        
        dropdown.ClearOptions();
        
        foreach (var resolution in _resolutions) { _filteredResolutions.Add(resolution); }
        List<string> options = new List<string>();
        foreach (var resolution in _filteredResolutions) {
            string resolutionOptions = resolution.width + "x" + resolution.height;
            options.Add(resolutionOptions);
        }
        
        dropdown.AddOptions(options);
        dropdown.value = _currentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    public void SetCurrentRes(int value) {
        Resolution res = _filteredResolutions[value];
        Screen.SetResolution(res.width, res.height, true);
        _saveManager.SaveData(SaveKeywords.ResolutionIndexKey, value);
    }

    public void SetFullScreen(bool isFullScreen) {
        _fullScreen = isFullScreen;
        Screen.fullScreen = _fullScreen;
        _saveManager.SaveData(SaveKeywords.IsFullScreenKey, _fullScreen);
    }
}
