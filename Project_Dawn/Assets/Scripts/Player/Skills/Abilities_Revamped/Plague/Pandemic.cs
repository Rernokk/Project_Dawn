using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pandemic : Ability
{
  public override void Activate()
  {
    print("Pandemic");
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Refreshes diseases, forces disease spread to nearby targets.
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
