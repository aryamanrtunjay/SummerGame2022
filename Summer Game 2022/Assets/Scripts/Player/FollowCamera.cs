using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Get player position from the inspector
    [SerializeField] private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Assign camera pos to variable to make individual values mutable
        Vector3 camPos = transform.position;

        // If player is 5 units away from camera, then follow
        if (Vector3.Distance(playerTransform.position, camPos) >= 5)
        {
            // Change camera pos by 1/100th of distance between player and camera (smoothens follow)
            camPos.x += (playerTransform.position.x - camPos.x) * 0.01f;
            camPos.y += (playerTransform.position.y - camPos.y) * 0.01f;
        }
        // Assign object transform to updated cam pos variable
        transform.position = camPos;
    }
}
