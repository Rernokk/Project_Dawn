using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ifrit_FSM : MonoBehaviour {
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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
