using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationRun : MonoBehaviour
{
    public Vector3 position;
    private Vector3 _currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        _currentPosition = this.transform.position;
    }

    private void Update()
    {
        // Calculate the new position
        _currentPosition = this.transform.position;
        var pos = _currentPosition + position;

        // Give game object the new position
        if (pos != _currentPosition)
            this.transform.position = pos;
    }
}