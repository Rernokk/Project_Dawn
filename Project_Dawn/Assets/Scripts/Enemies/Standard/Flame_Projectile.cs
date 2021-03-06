﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_Projectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(.5f,.5f,0))) > 30)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.transform.name);
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player_Controller>().Damage(25);
        }
        Destroy(gameObject);
    }
}
