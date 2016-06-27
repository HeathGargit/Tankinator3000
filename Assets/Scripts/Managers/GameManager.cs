/*---------------------------------------------------------
File Name: GameManager.cs
Purpose: To keep track of the "gamey" stuff, like winning and losing and what to do with scores.
Author: Heath Parkes (gargit@gargit.net)
Modified: 20/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //creates high scores object for access to loading/saving scores to disk.
    public HighScores m_HighScores;

    //For keeping track of all the tanks in the game
    public GameObject[] m_Tanks;

    //for keeping track of how quickly the player kills the enemy tank
    private float m_gameTime = 0f;
    public float GameTime {  get { return m_gameTime; } }

    //Reference to the overlay Text to display winning text, etc
    public Text m_MessageText;
    public Text m_TimerText;

    //Menu Text Setup
    public GameObject m_HighScorePanel;
    public Text m_HighScoresText;

    //Menu button setup
    public Button m_NewGameButton;
    public Button m_HighScoresButton;

    //an object containing game states for using in state tracking during game.
    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    //creating on of the afforementioned objects that tracks game state
    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    private void Awake()
    {
        // sets the GameState object to an inital value.
        m_GameState = GameState.Start;
    }

    private void Start()
    {
        //Inistialises game stuff. Everything is set to off as it isn't used until the game is playing or finished.
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
        m_MessageText.text = "Press Enter When Ready!";

        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // checking to see what our game is doing.
        switch (m_GameState)
        {
            //before the first game starts. once enter is pushed, the game starts
            case GameState.Start:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    //changing the state of things because the game has started.
                    m_TimerText.gameObject.SetActive(true);
                    m_MessageText.text = "";
                    m_GameState = GameState.Playing;

                    //activating tanks
                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;

            //As the game is playing, this is run. checks to see if the game has been won or lost.
            //Also does maitenance stuff like updating the time played
            case GameState.Playing:
                bool isGameOver = false;

                //updated the "played time"
                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));

                //checking to see if the enemy tank has died.
                if (OneTankLeft() == true)
                {
                    isGameOver = true;
                }
                else if (IsPlayerDead() == true) //checks to see if the player died.
                {
                    isGameOver = true;
                }

                //checks if the game is over, and shows the post game review screen.
                if (isGameOver == true)
                {
                    //change the game state to end-game
                    m_GameState = GameState.GameOver;
                    m_TimerText.gameObject.SetActive(false);

                    //show the end game text stuff.
                    m_NewGameButton.gameObject.SetActive(true);
                    m_HighScoresButton.gameObject.SetActive(true);

                    //display a message based on if the player died or if they killed the enemy tank
                    if (IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN!";
                    }
                    else
                    {
                        m_MessageText.text = "WINNER!!!";
                        
                        //Save the score
                        m_HighScores.AddScore(seconds);
                        m_HighScores.SaveScoresToFile();
                    }
                }
                break;

            //When the game is in it's end-game state, what to do when the player wants to play again.
            case GameState.GameOver:
                //Once enter is pushed, re-initialise everything back to it's start state.
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_gameTime = 0;
                    m_GameState = GameState.Playing;
                    m_MessageText.text = "";
                    m_HighScorePanel.gameObject.SetActive(false);
                    m_NewGameButton.gameObject.SetActive(false);
                    m_HighScoresButton.gameObject.SetActive(false);
                    m_TimerText.gameObject.SetActive(true);

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;
        }

        //A way fo quitting the game.
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //a function to check how many tanks are left, for game ending purposes.
    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        //count the amount of "alive" tanks
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }

        //return the number of tanks left.
        return numTanksLeft <= 1;
    }

    private bool IsPlayerDead()
    {
        //checks all tanks to see if they're dead, and then if they are, if they're the player.
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == false)
            {
                if (m_Tanks[i].tag == "Player")
                {
                    //if the tank is dead and it's the player, return true
                    return true;
                }
            }
        }

        //if the player tank isn't dead, return false.
        return false;
    }

    //method for resetting the game to it's starting state
    public void OnNewGame()
    {
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.gameObject.SetActive(false);

        m_gameTime = 0;
        m_GameState = GameState.Playing;
        m_TimerText.gameObject.SetActive(true);
        m_MessageText.text = "";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(true);
        }
    }

    //Handling when the "high Scores" button is pushed post-game
    public void OnHighScores()
    {
        m_MessageText.text = "";

        //show the high scores panel
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.gameObject.SetActive(true);

        //generate the high scores text to show
        string text = "";

        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n", (seconds / 60), (seconds % 60));
        }

        //I added this because it was hard to tell how to restart the game form here.
        text += "Press enter to restart!";

        //add all this text to the UI element
        m_HighScoresText.text = text;
    }
}
