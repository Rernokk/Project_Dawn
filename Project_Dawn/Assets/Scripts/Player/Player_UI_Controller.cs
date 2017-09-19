using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Controller : MonoBehaviour
{
    Player_Controller playerDetails;
    Image healthUI;

    [SerializeField]
    Material healthRefMat;
    Transform inventoryUI, InstructionsUI;
    Transform firstRow, secondRow, thirdRow;

    [SerializeField]
    List<Item> myList = new List<Item>();

    public int startVal = 0;
    public string currentType = "None";
    Button nextPage, prevPage;
    // Use this for initialization
    void Start()
    {
        playerDetails = GameObject.Find("Player").GetComponent<Player_Controller>();
        healthUI = transform.Find("HealthBar").GetComponent<Image>();
        InstructionsUI = transform.Find("Instructions");
        inventoryUI = transform.Find("Inventory/Inventory_Controller");
        firstRow = inventoryUI.Find("Top_Item");
        secondRow = inventoryUI.Find("Middle_Item");
        thirdRow = inventoryUI.Find("Bottom_Item");
        healthUI.material = new Material(healthRefMat);
        UpdateHealthValue();
        UpdateStats();
        IfNull();
        nextPage = GameObject.Find("Inventory_Controller/Next_Page").GetComponent<Button>();
        prevPage = GameObject.Find("Inventory_Controller/Previous_Page").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthValue();
    }

    public void UpdateHealthValue()
    {
        healthUI.material.SetFloat("_Value", playerDetails.HealthPercent);
    }

    public void Populate(string item)
    {
        currentType = item;
        //Reset
        myList.Clear();
        //Populate list by Item Type
        IEnumerable<Item> q = from thisItem in playerDetails.myInventory where thisItem.itemSlot == item select thisItem;
        foreach (Item i in q)
        {
            myList.Add(i);
        }
        myList.Sort();

        //Updating UI Controls appropriately for navigation.
        if (startVal == 0)
        {
            prevPage.interactable = false;
            nextPage.interactable = true;
        }
        else if (startVal == myList.Count - 2)
        {
            nextPage.interactable = false;
            prevPage.interactable = true;
        }
        else
        {
            nextPage.interactable = true;
            prevPage.interactable = true;
        }

        if (myList.Count < 3)
        {
            nextPage.interactable = false;
            prevPage.interactable = false;
        }

        //Changing and displaying appropriately.
        if (myList.Count > startVal)
        {
            firstRow.Find("myText").GetComponent<Text>().text = myList[startVal].myName;
            firstRow.Find("power").GetComponent<Text>().text = myList[startVal].Power.ToString();
            firstRow.Find("power/defense").GetComponent<Text>().text = myList[startVal].Defense.ToString();
            firstRow.GetComponent<Equipment_Item_Slot>().myItem = myList[startVal];
        }
        else
        {
            firstRow.Find("myText").GetComponent<Text>().text = "None";
            firstRow.Find("power").GetComponent<Text>().text = "";
            firstRow.Find("power/defense").GetComponent<Text>().text = "";
            secondRow.Find("myText").GetComponent<Text>().text = "None";
            secondRow.Find("power").GetComponent<Text>().text = "";
            secondRow.Find("power/defense").GetComponent<Text>().text = "";
            thirdRow.Find("myText").GetComponent<Text>().text = "None";
            thirdRow.Find("power").GetComponent<Text>().text = "";
            thirdRow.Find("power/defense").GetComponent<Text>().text = "";
        }

        if (myList.Count > startVal + 1)
        {
            secondRow.Find("myText").GetComponent<Text>().text = myList[startVal + 1].myName;
            secondRow.Find("power").GetComponent<Text>().text = myList[startVal + 1].Power.ToString();
            secondRow.Find("power/defense").GetComponent<Text>().text = myList[startVal + 1].Defense.ToString();
            secondRow.GetComponent<Equipment_Item_Slot>().myItem = myList[startVal + 1];
        }
        else
        {
            secondRow.Find("myText").GetComponent<Text>().text = "None";
            secondRow.Find("power").GetComponent<Text>().text = "";
            secondRow.Find("power/defense").GetComponent<Text>().text = "";
            thirdRow.Find("myText").GetComponent<Text>().text = "None";
            thirdRow.Find("power").GetComponent<Text>().text = "";
            thirdRow.Find("power/defense").GetComponent<Text>().text = "";
        }

        if (myList.Count > startVal + 2)
        {
            thirdRow.Find("myText").GetComponent<Text>().text = myList[startVal + 2].myName;
            thirdRow.Find("power").GetComponent<Text>().text = myList[startVal + 2].Power.ToString();
            thirdRow.Find("power/defense").GetComponent<Text>().text = myList[startVal + 2].Defense.ToString();
            thirdRow.GetComponent<Equipment_Item_Slot>().myItem = myList[startVal + 2];
        }
        else
        {
            thirdRow.Find("myText").GetComponent<Text>().text = "None";
            thirdRow.Find("power").GetComponent<Text>().text = "";
            thirdRow.Find("power/defense").GetComponent<Text>().text = "";
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
        //Nulling out empty slots.
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
        }
        else
        {
            thirdRow.GetComponent<Button>().interactable = true;
        }
    }
}
