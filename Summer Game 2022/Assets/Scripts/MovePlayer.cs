using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Vector3 containing change in movement
    private Vector3 dp;
    private bool isGrounded = false;

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


        // Ground Friction
        if(!Input.GetKey("a") && !Input.GetKey("d"))
        {
            if(dp.x < -0.01)
            {
                dp.x += 0.008f;
            }
            else if(dp.x > 0.01)
            {
                dp.x -= 0.008f;
            }
            else
            {
                dp.x = 0f;
            }
        }

        // Increase Y velocity if D is pressed by changing change in X
        if (Input.GetKey("space") && isGrounded)
        {
            dp.y = 10f;
            isGrounded = false;
        }

        if (isGrounded)
        {
            dp.y = 0f;
        }
        

        // Change player position by change in position (multiply by Time.deltaTime to make frame independent)
        transform.position += dp * Time.deltaTime;
    }

    void onCollisionEnter2D(Collision col)
    {
        Debug.Log("Hello");
        if(col.gameObject.tag == "Walkable")
        {
            isGrounded = true;
        }
    }
}
