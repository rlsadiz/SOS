using UnityEngine;
using System;
using System.Collections;

// SOS!
// Created by: Ronald Louie R. Sadiz
// Copyright 2015
// All Rights Reserved.

public class SpawnBlocks : MonoBehaviour
{
    public float zLevel;

    private ArrayList objectTriggers;
    private GameController gameController;

    void Start()
    {
        objectTriggers = new ArrayList();
        gameController = GetGameController();
    }

    void Update()
    {
        if (objectTriggers.Contains("Game Area") && !objectTriggers.Contains("Blocks"))
        {
            if (Input.GetButtonDown("Block S"))
                InstantiateBlock("S");
            else if (Input.GetButtonDown("Block O"))
                InstantiateBlock("O");
        }
    }
        
    void OnTriggerEnter(Collider other)
    {
        if (!objectTriggers.Contains(other.tag))
            objectTriggers.Add(other.tag);
    }

    void OnTriggerExit(Collider other)
    {
        objectTriggers.Remove(other.tag);
    }

    //Private methods
    private GameController GetGameController()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            return gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find 'GameController' script");
            return null;
        }
    }

    private void InstantiateBlock(String blockName)
    {

        Vector3 position = transform.position;
        position.z = zLevel;

        //Instantiate the object after verification
        GameObject block = Resources.Load("block_" + blockName, typeof(GameObject)) as GameObject;
        Instantiate(block, position, Quaternion.identity);

        //Notify Game Contoller
        gameController.NotifySpawnLocation(position, blockName);
    }
}