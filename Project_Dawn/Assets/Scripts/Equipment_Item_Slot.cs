﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_Item_Slot : MonoBehaviour {
    public Item myItem;
    Player_Controller player;
    Player_UI_Controller uiCtrl;
    [SerializeField]
    string myType;
    [SerializeField]
    bool ChangeText = true;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controller>();
        uiCtrl = GameObject.Find("PlayerUI").GetComponent<Player_UI_Controller>();
        if (ChangeText) { 
            transform.Find("myText").GetComponent<Text>().text = myItem.myName;
        }
    }
    public void SwapItem()
    {
        if (myItem.myName != "None")
        {
            Item temp = player.UpdateGear(myItem);
            myItem = temp;
            transform.Find("myText").GetComponent<Text>().text = myItem.myName;
            uiCtrl.UpdateStats();
        }
    }

    public void FetchList()
    {
        uiCtrl.Populate(myType);
    }
}
