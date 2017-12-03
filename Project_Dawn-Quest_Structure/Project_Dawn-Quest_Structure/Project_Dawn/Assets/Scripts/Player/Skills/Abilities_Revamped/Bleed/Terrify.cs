using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemies within range have any applied bleeds accelerated.
public class Terrify : Ability
{
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print(skillName);
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }
}
