using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Score")]
    public int score = 0;
    public Text scoreText;
    
    [Header("Timer")]
    public float totalTime = 60f; // Total time for the countdown
    public Text timerText; // Reference to the UI text element to display the timer
    
    [Header("Pause Menu")]
    public GameObject pauseMenu;
    
    private float _timeRemaining; // Time remaining for the countdown
    private bool _timerRunning; // Flag to check if the timer is running
    
    private bool _isPaused = false;

    private FirstPersonController _fpsController;
    
    void Start()
    {
        _fpsController = GetComponent<FirstPersonController>();
        
        _timeRemaining = totalTime; // Initialize the time remaining
    }
    
    void Update()
    {
        scoreText.text = "Score: " + score;

        if (_isPaused)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_isPaused)
            {
                Unpause();
            }
            else
            {
                PauseGame();
            }
        }
        
        // if escape go back to main menu
        
        if (_timerRunning)
        {
            // Update the time remaining
            _timeRemaining -= Time.deltaTime;

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);

            // Update the UI text to display the time remaining in minutes and seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            // 
            if (minutes < 0 || seconds < 0)
            {
                timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
            }

            // Check if the timer has reached zero
            if (_timeRemaining <= 0)
            {
                GameWon();
            }
        }
    }

    private void GameWon()
    {
        _timerRunning = false; // Stop the timer
        
        Debug.Log("Game Over");
        
        // display a different panel with the score and some messages to return to menu and stuff
                
        StopGameExecution();
    }
    
    private void PauseGame()
    {
        StopGameExecution();
    }

    private void StopGameExecution()
    {
        _isPaused = true;
        
        Time.timeScale = 0f;

        _fpsController.enabled = false;
        
        pauseMenu.SetActive(true);
    }
    
    private void Unpause()
    {
        _isPaused = false;
        
        Time.timeScale = 1f;
        
        _fpsController.enabled = true;
        
        pauseMenu.SetActive(false);
    }
    
    public void StartGame()
    {
        Unpause();
        
        _timerRunning = true; // Start the timer
        
        pauseMenu.SetActive(false);
    }
    
    public void ResetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        
        StartGame();
    }
    
    public void UpdateScore(int valueToAdd)
    {
        score += valueToAdd;
    }
}
