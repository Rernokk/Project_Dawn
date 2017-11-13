using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dread : Ability {
  public override void Activate()
  {
    print("Dread");
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Reduce enemy power over time, decreasing as duration passes.
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
