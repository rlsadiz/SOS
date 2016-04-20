using UnityEngine;
using System.Collections;

// SOS!
// Created by: Ronald Louie R. Sadiz
// Copyright 2015
// All Rights Reserved.

public class DragObjectByMouse : MonoBehaviour {

    Vector3 screenPoint;
    Vector3 offset;
    void OnMouseDown()
    {
        //translate the object's position from world to screen point
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        //calculate any difference between the object's position and 
        //mouse's screen position converted to world point
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        //keep track of mouse's position
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        //convert the mouse's screen position to world point and adjust with offset
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;

        //update the position of the object in the world
        transform.position = currentPosition;
    }
}
