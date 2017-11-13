using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increases movement speed, accelerated cooldowns.
public class AdrenalineRush : Ability
{
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print("Adrenaline Rush");
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    
  }
}
