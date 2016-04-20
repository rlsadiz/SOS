using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

// SOS!
// Created by: Ronald Louie R. Sadiz
// Copyright 2015
// All Rights Reserved.

public class GameController : MonoBehaviour {

    //Public variables
    public int boardSize;

    //Player Representation
    private int blueScore, redScore, score, turn;

    //Game Status Flags
    private bool spawned, scored, use_turn;

    //Game World Representation
    private BoxCollider boardArea;
    private int blockCount, x, y;
    private String[,] blocks;
    private int[,] adjacencyMatrix;

    //Diplay Text
    private Text turnText;
    private Text winText, gameOverText, restartText;
    private Text redScoreText, blueScoreText;

    //Computation
    private Vector3 boardOffset;
    private Vector3 boardLocation;

    //Unity Inherited Methods
    void Start()
    {
        StartGame();
    }

	void Update()
    {
        PlayerUpdate();
        GameWorldUpdate();

        if (!IsPlayerTurn())
            NextTurn();

        if (IsGameOver())
            EndGame();

        if (IsRestart())
            RestartGame();
    }

    //Initialization methods
    private void Initialize()
    {
        //Player data (turn = 0 is blue) 
        blueScore = 0;
        redScore = 0;
        score = 0;
        turn = 0;

        //Game flags 
        spawned = false;
        scored = false;
        use_turn = false;

        //Abstract representation of the board
        blockCount = 0;
        blocks = new String[boardSize, boardSize];
        adjacencyMatrix = new int[boardSize * boardSize, boardSize * boardSize];

        //Game Object Components
        boardArea = GameObject.FindWithTag("Game Area").GetComponent<BoxCollider>();
        turnText = GameObject.Find("turnText").GetComponent<Text>();
        winText = GameObject.Find("winText").GetComponent<Text>();
        gameOverText = GameObject.Find("gameOverText").GetComponent<Text>();
        restartText = GameObject.Find("restartText").GetComponent<Text>();
        redScoreText = GameObject.Find("redScoreText").GetComponent<Text>();
        blueScoreText = GameObject.Find("blueScoreText").GetComponent<Text>();

        boardOffset = boardArea.bounds.min;

        Debug.Log("Initialization Complete");
    }

    //Public Notify Methods
    public void NotifySpawnLocation(Vector3 spawnLocation, String blockName)
    {
        boardLocation = spawnLocation + new Vector3(0, 0, 1);
        AddBlockToGrid(blockName);

        use_turn = true;
        spawned = true;
        blockCount++;
    }

    //Game Logic - Process Methods
    private void StartGame()
    {
        Initialize();
        DisplayTurn();
        DisplayScore();
    }

    private void EndGame()
    {
        DisplayGameOver();
    }

    private void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    //Game Logic - Update Methods
    private void PlayerUpdate()
    {
        AddScore(score);
    }

    private void GameWorldUpdate()
    {
        CheckPlayerScore();

        spawned = false;
    }

    private void NextTurn()
    {
        turn = (turn + 1) % 2;
        use_turn = false;

        DisplayTurn();
    }

    //Game Logic - Status Methods
    private bool IsPlayerTurn()
    {
        if (use_turn)
            return scored;

        return true;
    }

    private bool IsGameOver()
    {
        return blockCount >= boardSize * boardSize;
    }

    private bool IsRestart()
    {
        if(Input.GetButtonDown("Restart"))
            return true;
        else
            return false;
    }

    //Game Logic - Helper Methods
    private void AddBlockToGrid(String blockName)
    {
        Vector3 gridPosition = boardLocation - boardOffset;
        x = (int) gridPosition.x;
        y = (int) gridPosition.y;

        if (blockName.Equals("S"))
            blocks[x, y] = "S";
        else if (blockName.Equals("O"))
            blocks[x, y] = "O";
    }

    private void AddLines(int x, int y, int type)
    {
        Vector3 position = new Vector3(x + 0.5f, y + 0.5f, -3);
        position = position + boardOffset;

        switch (turn)
        {
            case 0:
                InstanceLine(position, type, "blue");
                break;
            case 1:
                InstanceLine(position, type, "red");
                break;
        }
    }

    private void AddScore(int score)
    {
        switch (turn)
        {
            case 0:
                blueScore += score;
                break;
            case 1:
                redScore += score;
                break;
        }

        DisplayScore();
    }

    private void CheckPlayerScore()
    {
        score = 0;
        if (spawned)
        {
            if (blocks[x, y] == "S")
                CheckScoreBlockS(x, y);
            else if (blocks[x, y] == "O")
                CheckScoreBlockO(x, y);

            if (score > 0)
                scored = true;
            else
                scored = false;
        }
    }
    
    private void CheckScoreBlockS(int x, int y)
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 S = boardLocation + GetOffsetCoordinates(i) * 2;
            Vector3 O = boardLocation + GetOffsetCoordinates(i);

            if (boardArea.bounds.Contains(S) &&
                boardArea.bounds.Contains(O))
            {
                int SX = (int) (S - boardOffset).x;
                int SY = (int) (S - boardOffset).y;
                int OX = (int) (O - boardOffset).x;
                int OY = (int) (O - boardOffset).y;
                int adjX = x * boardSize + y;
                int adjY = SX * boardSize + SY;

                if (blocks[SX, SY] == "S" &&
                    blocks[OX, OY] == "O" &&
                    adjacencyMatrix[adjX, adjY] == 0)
                {
                    adjacencyMatrix[adjX, adjY] = 1;
                    adjacencyMatrix[adjY, adjX] = 1;
                    score++;
                    AddLines(OX, OY, i % 4);
                }
            }
        }
    }

    private void CheckScoreBlockO(int x, int y)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 S1 = boardLocation + GetOffsetCoordinates(i);
            Vector3 S2 = boardLocation + GetOffsetCoordinates(i + 4);

             if (boardArea.bounds.Contains(S1) &&
                 boardArea.bounds.Contains(S2))
             {
                 int S1X = (int) (S1 - boardOffset).x;
                 int S1Y = (int) (S1 - boardOffset).y;
                 int S2X = (int) (S2 - boardOffset).x;
                 int S2Y = (int) (S2 - boardOffset).y;
                 int adjX = S1X * boardSize + S1Y;
                 int adjY = S2X * boardSize + S2Y;

                 if (blocks[S1X, S1Y] == "S" &&
                     blocks[S2X, S2Y] == "S" &&
                     adjacencyMatrix[adjX, adjY] == 0)
                 {
                     adjacencyMatrix[adjX, adjY] = 1;
                     adjacencyMatrix[adjY, adjX] = 1;
                     score++;
                     AddLines(x, y, i % 4);
                 }
             }
        }
    }

    private int EvaluateFormula(int count)
    {
        if (count % 4 == 0)
            return 0;
        else
            return Convert.ToInt32(Math.Pow(-1, count / 4));
    }

    private Vector3 GetOffsetCoordinates(int count)
    {
        Vector3 returnVal;
        returnVal.x = EvaluateFormula(count + 2);
        returnVal.y = EvaluateFormula(count);
        returnVal.z = 0;
        return returnVal;
    }

    //Rendering Methods
    private void DisplayTurn()
    {
        switch(turn)
        {
            case 0:
                turnText.text = "Blue's Turn";
                turnText.color = Color.blue;
                break;
            case 1:
                turnText.text = "Red's Turn";
                turnText.color = Color.red;
                break;
        }
    }

    private void DisplayScore()
    {
        redScoreText.text = "Red: " + redScore;
        blueScoreText.text = "Blue: " + blueScore;
    }

    private void DisplayGameOver()
    {
        gameOverText.text = "Game Over";
        restartText.text = "Press R for Restart";

        if (blueScore > redScore)
        {
            winText.text = "Blue Wins";
            winText.color = Color.blue;
        }
        else if(blueScore < redScore)
        {
            winText.text = "Red Wins";
            winText.color = Color.red;
        }
        else
        {
            winText.text = "Tie";
            winText.color = Color.black;
        }

    }

    private void InstanceLine(Vector3 position, int type, String color)
    {
        GameObject line = Resources.Load("line_" + color, typeof(GameObject)) as GameObject;
        switch (type)
        {
            case 0:
                line.transform.localScale = new Vector3(0.5f, 0.35f, 0.5f);
                Instantiate(line, position, Quaternion.Euler(0, 0, 90));
                break;
            case 1:
                line.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Instantiate(line, position, Quaternion.Euler(0, 0, -45));
                break;
            case 2:
                line.transform.localScale = new Vector3(0.5f, 0.35f, 0.5f);
                Instantiate(line, position, Quaternion.Euler(0, 0, 0));
                break;
            case 3:
                line.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Instantiate(line, position, Quaternion.Euler(0, 0, 45));
                break;
        }
    }
}