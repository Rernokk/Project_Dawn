using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dread : Ability {
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print("Dread");
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Reduce enemy power over time, decreasing as duration passes.
}
