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

    void OnCollisionEnter2D(Collision2D col)
    {

        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 1f);
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 1f);
        RaycastHit2D down = Physics2D.Raycast(transform.position, Vector2.down, 1f);

        Debug.DrawRay(transform.position, Vector2.right, Color.green, 10000f);

        if (left.collider != null || right.collider != null)
        {
            
            dp.x = 0;
        }
        
        if(down.collider != null)
        {
            isGrounded = true;
        }
    }
}
