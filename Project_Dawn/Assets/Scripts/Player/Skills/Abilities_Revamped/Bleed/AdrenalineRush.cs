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
      StartCoroutine(BuffStats());
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    
  }

  IEnumerator BuffStats(){
    float val = playerController.playerSpeed * .25f;
    playerController.playerSpeed += val;
    abilityController.AccelerateCooldownRate(.25f);
    yield return new WaitForSeconds(6f);
    playerController.playerSpeed -= val;
    abilityController.AccelerateCooldownRate(-.25f);
  }
}
