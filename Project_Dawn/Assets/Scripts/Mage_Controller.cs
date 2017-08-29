using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_Controller : MonoBehaviour {
    [SerializeField]
    KeyCode first = KeyCode.Alpha1, second = KeyCode.Alpha2, third = KeyCode.Alpha3, fourth = KeyCode.Alpha4;

    Combo_System myComboSystem;

    KeyCode[] ComboArray;
    float[] DelayArray;
    

    void Start()
    {
        myComboSystem = GetComponent<Combo_System>();
        ComboArray = new KeyCode[3];
        ComboArray[0] = ComboArray[1] = ComboArray[2] = KeyCode.F;
        DelayArray = new float[3];
        DelayArray[0] = DelayArray[1] = DelayArray[2] = 1f;

    }

	void Update () {
		if (Input.GetKeyDown(first))
        {
            print("First SKill");
        }

        if (Input.GetKeyDown(second))
        {
            print("Second SKill");
            if (Input.GetKey(KeyCode.LeftShift))
            {
                print("Utility Skill");
            }
        }

        if (Input.GetKeyDown(third))
        {
            print("Third SKill");
        }

        if (Input.GetKeyDown(fourth))
        {
            print("Fourth SKill");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(myComboSystem.Combo(ComboArray, DelayArray));
        }

        //Display Teleport
        if (Input.GetKey(KeyCode.LeftShift))
        {
            print(Input.mousePosition);
            RaycastHit2D info = Physics2D.Raycast(Input.mousePosition, Vector2.down);
            if (info.transform != null)
            {
                print(info.transform.name);
            }
        }
	}
}
