// Filename:            PlayerOptions.cs
// Author:              Professor Wolfe
// Edited by:           Noah Stern
// Creation Date:       1/26/2026
// Last Modified:       3/26/2026
// Current Version:     0.3.0
// Previous Version:    0.2.1
// Project:             Sums & Sorcery (Script from Racing Game)
// Purpose:             A script that contains all of the player's settings for the given session
// Version Changes:     Yeah like here it is

using UnityEngine;
using UnityEngine.UIElements;

public static class PlayerOptions
{
    // The volume of the game's music
    public static float MusicVolume { get; set; } = 1;

    // The volume of the game's sound effects
    public static float SFXVolume { get; set; } = 1;

} 
