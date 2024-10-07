
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

    public Text scoreTextFinal;
    public Text welcomeText;
    public Text unpauseText;
    public Text resetText;
    
    private float _timeRemaining; // Time remaining for the countdown
    private bool _timerRunning; // Flag to check if the timer is running
    
    private bool _isPaused = false;
    private bool _isGameOver = false;
    
    private FirstPersonController _fpsController;
    
    void Start()
    {
        _fpsController = GetComponent<FirstPersonController>();
        
        _timeRemaining = totalTime; // Initialize the time remaining
        
        _timerRunning = true; // Start the timer
    }
    
    void Update()
    {
        scoreText.text = "Score: " + score;

        if (_isPaused && !_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
        }
        
        // disable key if the game is over
        if (!_isGameOver)
        {
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
        }

        // if (_isPaused || _isGameOver)
        // {
        //     if (Input.GetKeyDown(KeyCode.Escape))
        //     {
        //         _fpsController.enabled = false;
        //         SceneManager.LoadScene(0);
        //     }
        // }
        
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
        _isGameOver = true;
        
        welcomeText.text = "Game Over!";
        
        scoreTextFinal.gameObject.SetActive(true);
        scoreTextFinal.text = "Final Score: " + score;
        
        unpauseText.gameObject.SetActive(false);
        resetText.gameObject.SetActive(false);
                
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
        _isGameOver = false;
        
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
