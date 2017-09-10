using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_Chucker : Monster {
    [SerializeField]
    GameObject fireProjectile;

    [SerializeField]
    GameObject currentProjectile;

    // Use this for initialization
    void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (currentProjectile == null)
        {
            currentProjectile = Instantiate(fireProjectile, transform.position + direction * 1f + transform.up * .5f, Quaternion.identity);
            currentProjectile.transform.parent = transform;
        }

        if (!Triggered)
        {
            Aggro();
        } else
        {
            currentProjectile.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * 20f * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    protected override void Aggro()
    {
        RaycastHit2D[] info = Physics2D.RaycastAll(transform.position, transform.position - player.transform.position, 6f);
        bool inSight = true;
        bool inRange = false;
        foreach (RaycastHit2D hit in info)
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                inSight = false;
                break;
            }
            if (hit.transform.tag == "Player")
            {
                inRange = true;
            }
        }

        if (inSight && inRange)
        {
            Triggered = true;
            playerController.AddAggression(gameObject);
        }
    }

    public void Damage(float damageValue)
    {
        if (!Triggered)
        {
            Aggro();
        }

        base.Damage(damageValue);
    }
}
