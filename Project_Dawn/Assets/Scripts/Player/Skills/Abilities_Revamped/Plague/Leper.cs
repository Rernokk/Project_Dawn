using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leper : Ability
{
  public override void Activate()
  {
    print("Leper");
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Applies a strong disease to nearby enemies for a period of time, slows player.
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
