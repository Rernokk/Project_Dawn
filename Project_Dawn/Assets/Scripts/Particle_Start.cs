﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Start : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<ParticleSystem>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
