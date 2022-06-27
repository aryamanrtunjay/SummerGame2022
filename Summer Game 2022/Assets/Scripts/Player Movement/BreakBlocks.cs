using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlocks : MonoBehaviour
{
    public TerrainGenerationScript blocks;
    public float zOffSet = 20f;
    public float reach = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z += zOffSet;
            Vector3 direction = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;

            float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(mousePos), transform.position);
            if(distance > reach)
            {
                distance = reach;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);
            if (hit.collider != null && hit.collider.name != "BedRock")
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
