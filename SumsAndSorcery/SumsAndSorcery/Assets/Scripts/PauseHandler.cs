// Filename:            PauseHandler.cs
// Author:              Professor Wolfe
// Edited By:           Noah Stern
// Creation Date:       1/29/2026
// Last Modified:       3/26/2026
// Current Version:     0.2.2
// Previous Version:    0.2.1
// Project:             Sums & Sorcery (Script from Racing Game)
// Purpose:             Handles the pause menu
// Version Changes:     Commented the retry function and sens slider out

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseHandler : MonoBehaviour
{
    // The singleton class which directs all input actions
    private InputManager _inputManager;

    [SerializeField]
    [Tooltip("The gameobject containing the actual pause menu")]
    private GameObject _pauseMenu;

    // The audio menu field
    [SerializeField]
    [Tooltip("The gameobject containing the audio menu")]
    private GameObject _audioMenu;

    //[SerializeField]
    //private Slider _sensSlider;

    // Name:        Awake
    // Description: Called immediately upon the object loading in
    // Parameters:  None
    // Returns:     None
    void Awake()
    {
        _inputManager = InputManager.SingletonInstance;
        _inputManager.RegisterPulseAction("PauseMenu", ToggleMenu);
    }

    // Name:        Start
    // Description: Called immediately before the first execution of Update 
    // Parameters:  None
    // Returns:     None
    void Start()
    {
        _pauseMenu.SetActive(false);
    } 
// Name:        ToggleMenu
    // Description: Enables the menu if it is disabled and vice versa
    // Parameters:  None
    // Returns:     None
    public void ToggleMenu()
    {
        if (!_pauseMenu.activeSelf)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;   //added these two things
            _pauseMenu.SetActive(true);
            _audioMenu.SetActive(false);
            TimeManager.PauseGame();
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            _pauseMenu.SetActive(false);
            _audioMenu.SetActive(false);
            TimeManager.UnpauseGame();
        }
    }

    // Name:        ExitLevel
    // Description: Leaves the scene and returns to the main menu
    // Parameters:  None
    // Returns:     None
    public void ExitLevel()
    {
        TimeManager.UnpauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    // Name:        DisplayAudioSettings
    // Description: Opens the Audio menu and closes the pause menu
    // Parameters:  None
    // Returns:     None
    public void DisplayAudioSettings()
    {
        _pauseMenu.SetActive(false);
        _audioMenu.SetActive(true);
        //_sensSlider.value = PlayerOptions._sensitivity;

    }

    // Name:        BackToPauseMenu
    // Description: Closes the Audio menu and reopens the pause menu
    // Parameters:  None
    // Returns:     None
    public void BackToPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _audioMenu.SetActive(false);

    }

    // Name:        Retry
    // Description: Reloads the level
    // Parameters:  None
    // Returns:     None
    //public void Retry()
    //{
       // TimeManager.UnpauseGame();
        //string _currentRoom = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene(_currentRoom);
//    } 
// Name:        OnDestroy
    // Description: Called immediately before the object is destroyed
    // Parameters:  None
    // Returns:     None
    private void OnDestroy()
    {
        _inputManager.DeregisterPulseAction("PauseMenu", ToggleMenu);
    }
}