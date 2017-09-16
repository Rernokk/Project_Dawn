using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ifrit_FSM : Monster
{
    float auraMultiplier = 1f;

    public float AuraMult
    {
        get
        {
            return auraMultiplier;
        }

        set
        {
            auraMultiplier += value;
        }
    }

    protected override void Aggro()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Damage(float dmg)
    {
        Health -= dmg;
        myHealthMaterial.SetFloat("_Value", Health / TotalHealth);
        GameObject.Find("Monster_Ref").GetComponent<Monster_List_Ref>().CullMonster(this);
        if (Health<= 0)
        {
            GetComponent<Boss_Init>().ResetVariables();
            Destroy(gameObject);
        }
    }
}
