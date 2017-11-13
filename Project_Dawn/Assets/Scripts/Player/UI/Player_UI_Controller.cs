using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Controller : MonoBehaviour
{
  Player_Controller playerDetails;
  Image healthUI, manaUI, expUI;

  [SerializeField]
  Material healthRefMat, manaRefMat, expRefMat;
  CanvasGroup inventoryCanvas, instructionsCanvas;
  Transform firstRow, secondRow, thirdRow;
  Dictionary<string, CanvasGroup> uiTable;
  List<Item> inventoryList = new List<Item>();
  Text levelText;
  Image s1Skill, s2Skill, s3Skill, s4Skill;

  [HideInInspector]
  public int startVal = 0;
  [HideInInspector]
  public string currentType = "None";
  Button nextPage, prevPage;
  // Use this for initialization
  void Start()
  {
    uiTable = new Dictionary<string, CanvasGroup>();
    playerDetails = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();

    healthUI = transform.Find("Player_HUD/Overlay/HealthBar").GetComponent<Image>();
    manaUI = transform.Find("Player_HUD/Overlay/ManaBar").GetComponent<Image>();
    //expUI = transform.Find("Player_HUD/EXPBar").GetComponent<Image>();

    levelText = transform.Find("Player_HUD/Level").GetComponent<Text>();
    inventoryCanvas = transform.Find("Inventory").GetComponent<CanvasGroup>();

    uiTable.Add("Inventory", transform.Find("Inventory").GetComponent<CanvasGroup>());
    uiTable.Add("HUD", transform.Find("Player_HUD").GetComponent<CanvasGroup>());
    //uiTable.Add("Skills", transform.Find("Skill_Tree").GetComponent<CanvasGroup>());

    firstRow = inventoryCanvas.transform.Find("Inventory_Controller/Top_Item");
    secondRow = inventoryCanvas.transform.Find("Inventory_Controller/Middle_Item");
    thirdRow = inventoryCanvas.transform.Find("Inventory_Controller/Bottom_Item");

    healthUI.material = new Material(healthRefMat);
    manaUI.material = new Material(manaRefMat);
    //expUI.material = new Material(expRefMat);

    /*s1Skill = transform.Find("Player_HUD/CD_Overlay/S1Shadow/Skill").GetComponent<Image>();
    s2Skill = transform.Find("Player_HUD/CD_Overlay/S2Shadow/Skill").GetComponent<Image>();
    s3Skill = transform.Find("Player_HUD/CD_Overlay/S3Shadow/Skill").GetComponent<Image>();
    s4Skill = transform.Find("Player_HUD/CD_Overlay/S4Shadow/Skill").GetComponent<Image>();*/

    UpdateHealthValue();
    UpdateManaValue();
    //UpdateLevel();
    IfNull();

    nextPage = GameObject.Find("Inventory_Controller/Next_Page").GetComponent<Button>();
    prevPage = GameObject.Find("Inventory_Controller/Previous_Page").GetComponent<Button>();
  }

  // Update is called once per frame
  void Update()
  {
    UpdateHealthValue();
    UpdateManaValue();
    //UpdateSkillCooldowns();
  }

  public void UpdateLevel(){
    levelText.text = playerDetails.Level.ToString();
    UpdateSkills();
    //UpdateExpValue();
  }

  public void UpdateHealthValue()
  {
    healthUI.material.SetFloat("_Value", playerDetails.HealthPercent);
  }

  public void UpdateManaValue(){
    manaUI.material.SetFloat("_Value", playerDetails.ManaPercent);
  }

  public void UpdateExpValue(){
    //expUI.material.SetFloat("_Value", (float)playerDetails.currentExp / (float)playerDetails.TotalExp);
  }

  public void Populate(string item)
  {
    if (inventoryList == null){
      return;
    }

    currentType = item;
    //Reset
    inventoryList.Clear();
    //Populate list by Item Type
    if (inventoryList != null)
    {
      IEnumerable<Item> q = from thisItem in playerDetails.myInventory where thisItem.itemSlot == item select thisItem;

      foreach (Item i in q)
      {
        inventoryList.Add(i);
      }
      inventoryList.Sort();

      firstRow.GetComponent<Image>().color = Color.white;
      secondRow.GetComponent<Image>().color = Color.white;
      thirdRow.GetComponent<Image>().color = Color.white;

      //Updating UI Controls appropriately for navigation.
      if (startVal == 0)
      {
        prevPage.interactable = false;
        nextPage.interactable = true;
      }
      else if (startVal == inventoryList.Count - 2)
      {
        nextPage.interactable = false;
        prevPage.interactable = true;
      }
      else
      {
        nextPage.interactable = true;
        prevPage.interactable = true;
      }

      if (inventoryList.Count < 3)
      {
        nextPage.interactable = false;
        prevPage.interactable = false;
      }

      //Changing and displaying appropriately.
      if (inventoryList.Count > startVal)
      {
        firstRow.Find("myText").GetComponent<Text>().text = inventoryList[startVal].myName;
        firstRow.Find("power").GetComponent<Text>().text = inventoryList[startVal].Power.ToString();
        firstRow.Find("power/defense").GetComponent<Text>().text = inventoryList[startVal].Defense.ToString();
        firstRow.GetComponent<Equipment_Item_Slot>().myItem = inventoryList[startVal];
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

      if (inventoryList.Count > startVal + 1)
      {
        secondRow.Find("myText").GetComponent<Text>().text = inventoryList[startVal + 1].myName;
        secondRow.Find("power").GetComponent<Text>().text = inventoryList[startVal + 1].Power.ToString();
        secondRow.Find("power/defense").GetComponent<Text>().text = inventoryList[startVal + 1].Defense.ToString();
        secondRow.GetComponent<Equipment_Item_Slot>().myItem = inventoryList[startVal + 1];
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

      if (inventoryList.Count > startVal + 2)
      {
        thirdRow.Find("myText").GetComponent<Text>().text = inventoryList[startVal + 2].myName;
        thirdRow.Find("power").GetComponent<Text>().text = inventoryList[startVal + 2].Power.ToString();
        thirdRow.Find("power/defense").GetComponent<Text>().text = inventoryList[startVal + 2].Defense.ToString();
        thirdRow.GetComponent<Equipment_Item_Slot>().myItem = inventoryList[startVal + 2];
      }
      else
      {
        thirdRow.Find("myText").GetComponent<Text>().text = "None";
        thirdRow.Find("power").GetComponent<Text>().text = "";
        thirdRow.Find("power/defense").GetComponent<Text>().text = "";
      }
      if (inventoryList.Count > startVal && inventoryList[startVal].isNew)
      {
        firstRow.GetComponent<Image>().color = Color.yellow;
        inventoryList[startVal].isNew = false;
      }
      if (inventoryList.Count > startVal + 1 && inventoryList[startVal + 1].isNew)
      {
        secondRow.GetComponent<Image>().color = Color.yellow;
        inventoryList[startVal + 1].isNew = false;
      }
      if (inventoryList.Count > startVal + 2 && inventoryList[startVal + 2].isNew)
      {
        thirdRow.GetComponent<Image>().color = Color.yellow;
        inventoryList[startVal + 2].isNew = false;
      }
      IfNull();
    }
  }
  
  public void UpdateSkills(){
    CanvasGroup skillTree;
    uiTable.TryGetValue("Skills", out skillTree);
    foreach (Transform t in skillTree.transform){
      if (t.GetComponent<Skill_Specifier>())
      {
        t.GetComponent<Skill_Specifier>().UpdateInteractive(playerDetails.level);
      }
    }
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

  public void ToggleUIElementOff(string element)
  {
    CanvasGroup group;
    uiTable.TryGetValue(element, out group);
    group.alpha = 0;
    group.blocksRaycasts = false;
  }

  public void ToggleUIElementOn(string element)
  {
    CanvasGroup group;
    uiTable.TryGetValue(element, out group);
    group.alpha = 1;
    group.blocksRaycasts = true;
    if (group.GetComponent<UI_Select>())
    {
      group.GetComponent<UI_Select>().Sel();
    }
  }

  public void ToggleOffAllElements()
  {
    foreach (string key in uiTable.Keys)
    {
      ToggleUIElementOff(key);
    }
    ToggleUIElementOn("HUD");
  }

  public float IsElementActive(string element)
  {
    CanvasGroup group;
    uiTable.TryGetValue(element, out group);
    return group.alpha;
  }
}