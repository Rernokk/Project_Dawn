using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hemorage : Ability {
  //Apply a bleeding effect to the first target hit.
  public override void Initialize(){

  }

  public override void Activate()
  {
    print("Activating Hemorage");
  }

  public void AddFunctionality(){
    print("First Fire");
  }
}
