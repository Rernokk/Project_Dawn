using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : Ability {
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print("Decay");
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Lowers Defense, Applies a light disease.
}
