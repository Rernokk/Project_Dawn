using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Controller : MonoBehaviour {
    Player_Controller playerDetails;
    Image healthUI;

    [SerializeField]
    Material healthRefMat;
	// Use this for initialization
	void Start () {
        playerDetails = GameObject.Find("Player").GetComponent<Player_Controller>();
        healthUI = transform.Find("HealthBar").GetComponent<Image>();
        healthUI.material = new Material(healthRefMat);
        UpdateHealthValue();
	}
	
	// Update is called once per frame
	void Update () {
        healthUI.material.SetFloat("_Value", playerDetails.HealthPercent);
    }

    public void UpdateHealthValue()
    {
        healthUI.material.SetFloat("_Value", playerDetails.HealthPercent);
    }


}
