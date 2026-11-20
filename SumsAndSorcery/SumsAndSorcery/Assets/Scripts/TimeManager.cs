// Filename:            TimeManager.cs
// Author:              Professor Wolfe
// Creation Date:       1/29/2026
// Last Modified:       1/31/2026
// Current Version:     0.1.1
// Previous Version:    0.0.1
// Project:             Get-a-Grip (Script from Racing Game)
// Purpose:             A managerial wrapper class used to handle any and all changes to the timescale
// Version Changes:     Added comments and tooltips
using UnityEngine;

public class TimeManager
{
    // The regular timescale of the game
    private const float RegularTimeScale = 1f;

    // The timescale last recorded for when the game is to be unpaused
    private static float _unpausedTimeScale = 1f;

    // The input manager
    //private static InputManager _inputManager = InputManager.SingletonInstance;

    // Name:        PauseGame
    // Description: Pauses time, sound, and non-UI input
    // Parameters:  None
    // Returns:     None
    public static void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        //_inputManager.ToggleGameplayControls(false);
    }

    // Name:        UnpauseGame
    // Description: Enables time, sound, and non-UI input
    // Parameters:  None
    // Returns:     None
    public static void UnpauseGame()
    {
        Time.timeScale = _unpausedTimeScale;
        AudioListener.pause = false;
        //_inputManager.ToggleGameplayControls(true);
    }

    // Name:        ChangeTimeScale
    // Description: Changes the timescale to the desired value
    // Parameters:  float updatedTimeScale - The new timescale
    // Returns:     None
    public static void ChangeTimeScale(float updatedTimeScale)
    {
        _unpausedTimeScale = updatedTimeScale;
        Time.timeScale = updatedTimeScale;
    }

    // Name:        ResetTimeScale
    // Description: Resets the timescale to the standard
    // Parameters:  None
    // Returns:     None
    public static void ResetTimeScale()
    {
        _unpausedTimeScale = RegularTimeScale;
        Time.timeScale = RegularTimeScale;
    }

    //~ Not necessary: why?
    /*
    //private static AudioListener _activeListener;
    public static void SetAudioListener(AudioListener activeListener)
    {
        _activeListener = activeListener;
    }
    */
}

