using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pandemic : Ability
{
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print("Pandemic");
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Refreshes diseases, forces disease spread to nearby targets.
}
