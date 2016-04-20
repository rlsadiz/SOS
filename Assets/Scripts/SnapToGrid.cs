using UnityEngine;
using System.Collections;

// SOS!
// Created by: Ronald Louie R. Sadiz
// Copyright 2015
// All Rights Reserved.

public class SnapToGrid : MonoBehaviour {

    public float xDimension;
    public float yDimension;
    public float zDimension;

    void Update()
    {
        //gets the current position of the object
        Vector3 newPosition = transform.position;

        //translates the position to its nearest grid value
        newPosition.x = Mathf.Round(newPosition.x / xDimension) * xDimension;
        newPosition.y = Mathf.Round(newPosition.y / yDimension) * yDimension;
        newPosition.z = Mathf.Round(newPosition.z / zDimension) * zDimension;

        //transforms the objects position
        transform.position = newPosition;
    }
}
