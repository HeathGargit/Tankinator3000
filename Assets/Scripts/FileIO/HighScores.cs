/*---------------------------------------------------------
File Name: HighScores.cs
Purpose: To provide file reading and writing functinoality to save/read high scores.
Author: Heath Parkes (gargit@gargit.net)
Modified: 20/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class HighScores : MonoBehaviour {

    //To hold the high scores.
    public int[] scores = new int[10];

    //to store the directory where the file will be saved. It is not hard-coded as it changes per system type.
    string currentDirectory;

    //name of the file that will store high scores.
    public string scoreFileName = "highscores.txt";

	void Start ()
    {
        //We need to know where we are reading from and writing to.
        // To help us with that, we'll print the current directory to the console.
        currentDirectory = Application.dataPath;
        Debug.Log("Datapath: " + currentDirectory);

        //Load the scores by default
        LoadScoresFromFile();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //creates a "quick-load" high score console output for the F9 key
	    if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadScoresFromFile();
            foreach (int score in scores)
            {
                Debug.Log(score);
            }
        }

        //provides a "quick save" for the f10 key 
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SaveScoresToFile();
        }
	}

    //this function loads high scores from a file.
    public void LoadScoresFromFile()
    {
#if UNITY_STANDALONE
        //Before we try and read a file, we should check that it exists.
        //If it doesn't exist, we'll log a message and abort.
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists)
        {
            //output success message
            Debug.Log("Found High Score File " + scoreFileName);
        }
        else
        {
            //output "failure" message
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            return;
        }

        //Make a new array of default values. 
        //This ensures that no old values stick around if we've loaded a scores file in the past.
        scores = new int[scores.Length];

        //Now we read the file in. We do this using a StreamReader, which we give our full file path to.
        //Dont forge the directory separator between the directory and the filename!
        StreamReader fileReader;

        //file IO exception handling. In case something bad happens, usually related to file permissions
        try
        {
            fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);
        }
        catch (Exception e)
        {
            //if there's an error, write it to the console
            Debug.Log(e.Message);
            return;
        }
        

        //A counter to make sure we dont go past the end of our scores
        int scoreCount = 0;

        //A while loop, which  runs as long as there is data to be read AND we haven't reached the end of our scores array
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            //Read that line into a variable
            string fileLine = fileReader.ReadLine();

            //Try to parse that variable into an int
            //First make a variable to put it in
            int readScore = -1;
            // Try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                //if we successfully read a number, put it into the array
                scores[scoreCount] = readScore;
            }
            else
            {
                //if the number couldn't be parsed then we porbably had junk in our file.
                //Let's print an error, and then use a default value.
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);
                scores[scoreCount] = 0;
            }
            //Dont forget to increment the counter!
            scoreCount++;
        }

        //Remember to close the stream!
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
#endif
    }

    public void SaveScoresToFile()
    {
#if UNITY_STANDALONE
        //create the output stream
        StreamWriter fileWriter;

        try //try writing to the disk, handles an exception (usually file permission based)
        {
            fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);
        }
        catch (Exception e)
        {
            //if there's an error, show the error message
            Debug.Log(e.Message);
            return;
        }

        //Write the lines to the file
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        //Close the stream
        fileWriter.Close();

        //Write a message
        Debug.Log("High scores written to " + scoreFileName);
#endif
    }

    public void AddScore(int newScore)
    {
        //First up we find out what index it belongs at.
        //This will be the first index with a score lower than the new score.
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            //Instead of cehcking the value of desiredIndex we could aslo use "break" to stop the loop.
            if (scores[i] > newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        //if no desired index was found, then the score wasn't high enough to get on the table, so we just abort
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " was not high enough for high scores list.", this);
            return;
        }

        //Then we move all of the scores after that index back by one position. We'll do this by looping from the back of the array to our desired position
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        //Insert our new score in it's place
        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered into the high scores at position " + desiredIndex, this);
    }
}
