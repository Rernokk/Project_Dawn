using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour {
    protected GameObject player;
    protected Player_Controller playerController;
    protected bool Triggered = false;

    [SerializeField]
    protected int Health, TotalHealth = 100;

    public Color MyHealthColor;
    public Material HealthRefMat;

    protected Material myHealthMaterial;
    protected GameObject myHealthBar;
    // Use this for initialization
    protected void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<Player_Controller>();
        Health = TotalHealth;
        myHealthMaterial = new Material(HealthRefMat);
        myHealthMaterial.SetColor("_Color", MyHealthColor);
        myHealthBar = transform.Find("Canvas").Find("Health").gameObject;
        myHealthMaterial.SetFloat("_Value", Health/TotalHealth);
        myHealthBar.GetComponent<Image>().material = myHealthMaterial;
    }
	
    protected abstract void Aggro();
    protected void Damage (int damageValue)
    {
        Health -= damageValue;
        myHealthMaterial.SetFloat("_Value", (float) Health / TotalHealth);
    }
}
