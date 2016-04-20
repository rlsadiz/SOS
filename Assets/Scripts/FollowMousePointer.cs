using UnityEngine;
using System.Collections;

// SOS!
// Created by: Ronald Louie R. Sadiz
// Copyright 2015
// All Rights Reserved.

public class FollowMousePointer : MonoBehaviour {

    public float zLevel;

	// Update is called once per frame
	void Update() {
        //convert the mouse's screen position to world point
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPosition.z = zLevel;

        //update the position of the object in the world
        transform.position = currentPosition;
	}
}
