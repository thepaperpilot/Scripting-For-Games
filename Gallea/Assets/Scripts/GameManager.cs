using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int score = 0;
    public int targetScore = 400;
    public float resetDelay = 1f;
    public Text scoreText;
    public Text timeText;
    public int timePerLevel = 30;
    public GameObject youWon;
    public GameObject gameOver;

    private float clockSpeed = 1f;

    void Awake()
    {
        scoreText.text = "Score: " + score + "/" + targetScore;
        InvokeRepeating("Clock", 0, clockSpeed);
    }

    void Clock()
    {
        timePerLevel--;
        timeText.text = "Time: " + timePerLevel;
        if (timePerLevel == 0)
            CheckGameOver();
    }

    void CheckGameOver()
    {
        Time.timeScale = .25f;
        if (score >= targetScore)
        {
            youWon.SetActive(true);
            Invoke("Reset", resetDelay * .25f);
        }
        else
        {
            gameOver.SetActive(true);
            Invoke("Reset", resetDelay * .25f);
        }
    }

    void Reset()
    {
        Debug.Log("!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddPoints(int pointScored)
    {
        score += pointScored;
        scoreText.text = "Score: " + score + "/" + targetScore;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
