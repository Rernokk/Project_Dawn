using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leper : Ability
{
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print("Leper");
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  //Applies a strong disease to nearby enemies for a period of time, slows player.
}
