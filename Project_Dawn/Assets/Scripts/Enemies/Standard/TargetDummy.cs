using System.Collections;
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
        if (Health < TotalHealth)
        {
            Heal(10 * Time.deltaTime);
        }
    }

    public void Damage(float damageValue)
    {
        base.Damage(damageValue);
        if (!Triggered)
        {
            Aggro();
        }
    }

    protected override void Aggro()
    {
        Triggered = true;
        playerController.AddAggression(gameObject);
    }
}
