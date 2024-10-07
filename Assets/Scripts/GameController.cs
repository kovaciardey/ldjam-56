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

    public Text welcomeText;
    public Text startText;

    public GameObject startGameButton;
    
    private float _timeRemaining; // Time remaining for the countdown
    private bool _timerRunning; // Flag to check if the timer is running
    
    private bool _isPaused = false;

    public void UpdateScore(int valueToAdd)
    {
        score += valueToAdd;
    }
    
    void Start()
    {
        // start the game as paused
        InitialSetup();

        // _dialogs = new List<GameObject>();
        
        // StartGame();

        // _timerRunning = true;
    }
    
    void Update()
    {
        scoreText.text = "Score: " + score;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        
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

    private void InitialSetup()
    {
        // PauseGame();
        
        _timeRemaining = totalTime; // Initialize the time remaining

        welcomeText.text = "Get Ready!";
        startText.text = "BEGIN!";
    }

    private void GameWon()
    {
        _timerRunning = false; // Stop the timer
        
        Debug.Log("Game Over");
                
        StopGameExecution();
                
        welcomeText.text = "YOU WON!";
        startGameButton.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        
        Unpause();
        
        _timerRunning = true; // Start the timer
        
        pauseMenu.SetActive(false);
    }

    private void GameOver()
    {
        StopGameExecution();
        
        welcomeText.text = "Game Over!";
        startGameButton.SetActive(false);
    }
    
    private void PauseGame()
    {
        StopGameExecution();
        
        welcomeText.text = "Paused";
        startText.text = "Continue";
    }

    private void StopGameExecution()
    {
        _isPaused = true;
        
        Time.timeScale = 0f;
        
        pauseMenu.SetActive(true);
    }
    
    private void Unpause()
    {
        _isPaused = false;
        
        Time.timeScale = 1f;
    }

    public bool IsPaused()
    {
        return _isPaused;
    }

    public void ResetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        
        StartGame();
    }
}
