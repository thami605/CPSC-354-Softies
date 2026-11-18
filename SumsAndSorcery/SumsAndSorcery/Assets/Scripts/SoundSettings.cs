// Filename:            SoundSettings.cs
// Author:              Professor Wolfe
// Creation Date:       1/29/2026
// Last Modified:       2/1/2026
// Current Version:     0.1.1
// Previous Version:    0.0.1
// Project:            Sums & Sorcery (Script from Racing Game)
// Purpose:             Handles updates made to audio settings
// Version Changes:     Added comments and tooltips
//                      Renamed members to be more descriptive
//                      Moved slider updating to Start() to match project conventions
//                      Outsourced actual audio handling to AudioManager singleton

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Tooltip("The slider for SFX volume")]
    public Slider _sfxVolumeSlider;

    [Tooltip("The slider for music volume")]
    public Slider _musicVolumeSlider;

    // Singleton AudioManager for all actual changes to audio
    private AudioManager _audioManager;

    // Name:        Awake
    // Description: Called immediately upon the object loading in
    // Parameters:  None
    // Returns:     None
    void Awake()
    {
        _audioManager = AudioManager.SingletonInstance;
    }

    // Name:        Start
    // Description: Called immediately before the object's first Update frame
    // Parameters:  None
    // Returns:     None
    void Start()
    {
        _sfxVolumeSlider.value = PlayerOptions.SFXVolume;
        _musicVolumeSlider.value = PlayerOptions.MusicVolume;
    }

    // Name:        UpdateSFXVolume
    // Description: Forwards SFX volume changes to the audio manager
    // Parameters:  float updatedValue - The new SFX volume modifier
    // Returns:     None
    public void UpdateSFXVolume(float updatedValue)
    {
        _audioManager.UpdateSFXVolume(updatedValue);
    } 
// Name:        UpdateMusicVolume
    // Description: Forwards music volume changes to the audio manager
    // Parameters:  float updatedValue - The new music volume modifier
    // Returns:     None
    public void UpdateMusicVolume(float updatedValue)
    {
        _audioManager.UpdateMusicVolume(updatedValue);
    }
}