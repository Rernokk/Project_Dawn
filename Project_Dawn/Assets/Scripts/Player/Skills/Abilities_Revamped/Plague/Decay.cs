using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : Ability {
  public override void Activate()
  {
    print("Decay");
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Lowers Defense, Applies a light disease.
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
