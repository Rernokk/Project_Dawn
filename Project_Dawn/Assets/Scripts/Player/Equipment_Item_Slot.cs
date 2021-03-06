﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_Item_Slot : MonoBehaviour
{
  public Item myItem;
  Player_Controller player;
  Player_UI_Controller uiCtrl;
  [SerializeField]
  string myType;
  [SerializeField]
  bool ChangeText = true;
  private void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
    uiCtrl = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Player_UI_Controller>();
    if (ChangeText)
    {
      transform.Find("myText").GetComponent<Text>().text = myItem.myName;
    }
  }
  public void SwapItem()
  {
    if (myItem.myName != "None")
    {
      if (myItem.isNew)
      {
        myItem.isNew = !myItem.isNew;
      }
      Item temp = player.UpdateGear(myItem);
      myItem = temp;
      transform.Find("myText").GetComponent<Text>().text = myItem.myName;
      transform.Find("power").GetComponent<Text>().text = myItem.Power.ToString();
      transform.Find("power/defense").GetComponent<Text>().text = myItem.Defense.ToString();
      transform.GetComponent<Image>().color = Color.white;
    }
  }

  public void FetchList()
  {
    uiCtrl.startVal = 0;
    uiCtrl.Populate(myType);
  }

  public void ShiftList(int val)
  {
    uiCtrl.startVal += val;
    uiCtrl.Populate(uiCtrl.currentType);
  }
}
