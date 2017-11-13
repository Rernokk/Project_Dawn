using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viral : Ability
{
  public override void Activate()
  {
    print("Viral");
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Leeches power and defense from targets afflicted, increasing in strength by target health lost.
  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
