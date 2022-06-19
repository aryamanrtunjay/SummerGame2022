using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Vector3 containing change in movement
    private Vector3 dp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease X velocity if A is pressed by changing change in X
        if(Input.GetKey("a"))
        {
            dp.x -= 0.01f;
        }

        // Increase X velocity if D is pressed by changing change in X
        if (Input.GetKey("d"))
        {
            dp.x += 0.01f;
        }

        // Decrease X velocity if A is pressed by changing change in X
        if (Input.GetKey("s"))
        {
            dp.y -= 0.01f;
        }

        // Increase X velocity if D is pressed by changing change in X
        if (Input.GetKey("w"))
        {
            dp.y += 0.01f;
        }

        // Change player position by change in position (multiply by Time.deltaTime to make frame independent)
        transform.position += dp * Time.deltaTime;
    }
}
