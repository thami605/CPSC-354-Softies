// Filename:            SelectedNoisemaker.cs
// Author:              Professor Wolfe
// Creation Date:       1/29/2026
// Last Modified:       2/1/2026
// Current Version:     0.1.1
// Previous Version:    0.0.1
// Project:             Racing Game
// Purpose:             Handles sound effect generation given multiple options
// Version Changes:     Comments and tooltips added
//                      Sound generation customized with effectively a custom
//                        implementation of AudioSource.PlayClipAtPoint

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SelectedNoisemaker : MonoBehaviour
{
    [field: SerializeField]
    [Tooltip("All possible sound options which can be played")]
    public AudioClip[] PossibleSounds { get; private set; }

    [field: SerializeField]
    [Tooltip("The Audio Mixer Group for the sound set")]
    public AudioMixerGroup MixerGroup { get; private set; }

    // Name:        PlayRandomNoise
    // Description: Plays a random noise from the list of possible sounds at the selected location
    // Parameters:  Vector3 noiseLocation - The location that the noise should be played at
    // Returns:     GameObject - The GameObject of the spawned noise
    // Notes:       The code for this is very similar to AudioSource.PlayClipAtPoint. The only differences
    //                is this version returns the GameObject, can have a mixer, randomly chooses from
    //                PossibleSounds, and can be given a parent transform
    //              Efficiency of PlayClipAtPoint and similar options have been tested by leni8ec on Unity's
    //                discussion forum: https://discussions.unity.com/t/playoneshot-performance/595405/5
    public GameObject PlayRandomNoise(Vector3 noiseLocation, Transform parentObject)
    {
        GameObject soundEffectObject = new GameObject("One-shot Noise");
        //! Why is this false, I hear you say. Based on the description in the Unity docs, it seems that
        //!   setting the second parameter as true requires more code to be run. Unfortunately, I cannot
        //!   confirm that because Unity keeps that code secret
        soundEffectObject.transform.SetParent(parentObject, false);
        // Thus the second parameter of the above statement matters not
        soundEffectObject.transform.position = noiseLocation;

        AudioSource soundEffectSource = soundEffectObject.AddComponent<AudioSource>();
        soundEffectSource.clip = PossibleSounds[Mathf.RoundToInt(Random.value * (PossibleSounds.Length - 1))];
        soundEffectSource.outputAudioMixerGroup = MixerGroup;
        soundEffectSource.loop = false;
        soundEffectSource.volume = 1.0f;
        soundEffectSource.Play();
        //!! This does not account for timescale warps or pauses mid-way through, but Unity does not have
        //!!   a portable way of doing so. Alternate solutions could be to make a Coroutine and have it check
        //!!   the isplaying field upon completion, or update the lifespan anytime the TimeManager class is
        //!!   affected, but both are very inefficient and difficult to code. This should be fixed, but can
        //!!   wait for a future sprint
        Destroy(soundEffectObject, soundEffectSource.clip.length * Time.deltaTime);
        return soundEffectObject;
    }


    //~ Method overloading Efficiency discussion here
    // Name:        PlayRandomNoise
    // Description: Plays a random noise from the list of possible sounds at the selected location
    // Parameters:  Vector3 noiseLocation - The location that the noise should be played at
    // Returns:     GameObject - The GameObject of the spawned noise
    public GameObject PlayRandomNoise(Vector3 noiseLocation)
    {
        GameObject soundEffectObject = new GameObject("One-shot Noise");
        soundEffectObject.transform.position = noiseLocation;
        //# Line taken from AudioSource.PlayClipAtPoint
        AudioSource soundEffectSource = (AudioSource)soundEffectObject.AddComponent(typeof(AudioSource));
        soundEffectSource.clip = PossibleSounds[Mathf.RoundToInt(Random.value * (PossibleSounds.Length - 1))];
        soundEffectSource.outputAudioMixerGroup = MixerGroup;

        soundEffectSource.loop = false;
        soundEffectSource.volume = 1.0f;
        //# Line taken from AudioSource.PlayClipAtPoint
        soundEffectSource.spatialBlend = 1f;
        soundEffectSource.Play();
        //!! This does not account for timescale warps or pauses mid-way through, but Unity does not have
        //!!   a portable way of doing so. Alternate solutions could be to make a Coroutine and have it check
        //!!   the isplaying field upon completion, or update the lifespan anytime the TimeManager class is
        //!!   affected, but both are very inefficient and difficult to code. This should be fixed, but can
        //!!   wait for a future sprint
        //!! What can be done easily is allowing it to work with changes to the Audio Mixer, which it currently
        //!!   does not. Complete this
        Destroy(soundEffectObject, soundEffectSource.clip.length * Time.timeScale);
        return soundEffectObject;
    }
}