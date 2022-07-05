using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject slot1Obj;
    public GameObject slot2Obj;
    public GameObject slot3Obj;
    public GameObject slot4Obj;
    public GameObject slot5Obj;

    private RectTransform slot1;
    private RectTransform slot2;
    private RectTransform slot3;
    private RectTransform slot4;
    private RectTransform slot5;

    private Vector2 selectedSize = new Vector2(1.5f, 1.5f);
    private Vector2 notSelectedSize = new Vector2(1f, 1f);

    private int currSelected = 1;
    // Start is called before the first frame update
    void Start()
    {
        slot1 = slot1Obj.GetComponent<RectTransform>();
        slot2 = slot2Obj.GetComponent<RectTransform>();
        slot3 = slot3Obj.GetComponent<RectTransform>();
        slot4 = slot4Obj.GetComponent<RectTransform>();
        slot5 = slot5Obj.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currSelected);
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currSelected++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currSelected--;
        }

        if(currSelected < 1)
        {
            currSelected = 1;
        }
        else if(currSelected > 5)
        {
            currSelected = 5;
        }

        slot1.localScale = new Vector3(36f, 36f, 36f);

        slot2.localScale = new Vector3(36f, 36f, 36f);

        slot3.localScale = new Vector3(36f, 36f, 36f);

        slot4.localScale = new Vector3(36f, 36f, 36f);

        slot5.localScale = new Vector3(36f, 36f, 36f);

        if (currSelected == 1)
        {
            slot1.localScale = new Vector3(36f * 1.2f, 36f * 1.2f, 36f);
        }
        else if(currSelected == 2)
        {
            slot2.localScale = new Vector3(36f * 1.2f, 36f * 1.2f, 36f);
        }
        else if (currSelected == 3)
        {
            slot3.localScale = new Vector3(36f * 1.2f, 36f * 1.2f, 36f);
        }
        else if (currSelected == 4)
        {
            slot4.localScale = new Vector3(36f * 1.2f, 36f * 1.2f, 36f);
        }
        else if (currSelected == 5)
        {
            slot5.localScale = new Vector3(36f * 1.2f, 36f * 1.2f, 36f);
        }
        
    }
}
