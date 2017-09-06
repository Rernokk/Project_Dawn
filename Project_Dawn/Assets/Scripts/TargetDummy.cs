﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDummy : Monster {
	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public void Damage(float damageValue)
    {
        base.Damage(damageValue);
        if (!Triggered)
        {
            Aggro();
        }
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected override void Aggro()
    {
        Triggered = true;
        playerController.AddAggression(gameObject);
    }
}
