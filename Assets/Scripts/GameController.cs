using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int score = 0;

    public Text scoreText;

    private void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateScore(int valueToAdd)
    {
        score += valueToAdd;
        Debug.Log("Score");
    }
}
