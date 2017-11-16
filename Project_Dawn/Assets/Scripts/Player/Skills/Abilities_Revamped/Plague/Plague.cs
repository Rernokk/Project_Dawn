using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Ability
{
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print("Plague Activated");
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }
}
