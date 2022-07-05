using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Vector2 spawn;
    private int checker = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checker == 1){
            transform.position = spawn;
            Debug.Log(spawn);
        }
        checker+=1;
    }
}
