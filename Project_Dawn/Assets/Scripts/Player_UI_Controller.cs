using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Controller : MonoBehaviour {
    Player_Controller playerDetails;
    Image healthUI;

    [SerializeField]
    Material healthRefMat;
    Transform inventoryUI;
    Transform firstRow, secondRow, thirdRow;

    [SerializeField]
    List<Item> myList = new List<Item>();
    // Use this for initialization
    void Start () {
        playerDetails = GameObject.Find("Player").GetComponent<Player_Controller>();
        healthUI = transform.Find("HealthBar").GetComponent<Image>();
        inventoryUI = transform.Find("Inventory/Inventory_Controller").transform;
        firstRow = inventoryUI.Find("Top_Item");
        secondRow = inventoryUI.Find("Middle_Item");
        thirdRow = inventoryUI.Find("Bottom_Item");
        healthUI.material = new Material(healthRefMat);
        UpdateHealthValue();
        UpdateStats();
        IfNull();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHealthValue();
    }

    public void UpdateHealthValue()
    {
        healthUI.material.SetFloat("_Value", playerDetails.HealthPercent);
    }

    public void Populate(string item)
    {
        myList.Clear();
        IEnumerable<Item> q = from thisItem in playerDetails.myInventory where thisItem.itemSlot == item select thisItem;
        foreach (Item i in q)
        {
            myList.Add(i);
        }
        if (myList.Count > 0)
        {
            firstRow.Find("myText").GetComponent<Text>().text = myList[0].myName;
            firstRow.GetComponent<Equipment_Item_Slot>().myItem = myList[0];
        } else
        {
            firstRow.Find("myText").GetComponent<Text>().text = "None";
            secondRow.Find("myText").GetComponent<Text>().text = "None";
            thirdRow.Find("myText").GetComponent<Text>().text = "None";
        }

        if (myList.Count > 1)
        {
            secondRow.Find("myText").GetComponent<Text>().text = myList[1].myName;
            secondRow.GetComponent<Equipment_Item_Slot>().myItem = myList[1];
        } else
        {
            secondRow.Find("myText").GetComponent<Text>().text = "None";
            thirdRow.Find("myText").GetComponent<Text>().text = "None";
        }

        if (myList.Count > 2)
        {
            thirdRow.Find("myText").GetComponent<Text>().text = myList[2].myName;
            thirdRow.GetComponent<Equipment_Item_Slot>().myItem = myList[2];
        } else
        {
            thirdRow.Find("myText").GetComponent<Text>().text = "None";
        }
        IfNull();
        UpdateStats();
    }

    public void UpdateStats()
    {
        transform.Find("Inventory/Power").GetComponent<Text>().text = "Power \n" + playerDetails.Power.ToString();
        transform.Find("Inventory/Defense").GetComponent<Text>().text = "Defense \n" + playerDetails.Defense.ToString();
    }

    public void IfNull()
    {
        if (firstRow.Find("myText").GetComponent<Text>().text == "None")
        {
            firstRow.GetComponent<Button>().interactable = false;
        }
        else
        {
            firstRow.GetComponent<Button>().interactable = true;
        }

        if (secondRow.Find("myText").GetComponent<Text>().text == "None")
        {
            secondRow.GetComponent<Button>().interactable = false;
        }
        else
        {
            secondRow.GetComponent<Button>().interactable = true;
        }

        if (thirdRow.Find("myText").GetComponent<Text>().text == "None")
        {
            thirdRow.GetComponent<Button>().interactable = false;
        } else
        {
            thirdRow.GetComponent<Button>().interactable = true;
        }
    }
}
