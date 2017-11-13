using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Ability
{
  public override void Activate()
  {
    print("Plague");
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Applies a disease effect, has a chance to apply a stronger effect to nearby enemies.
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
