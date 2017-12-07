using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increases movement speed, accelerated cooldowns.
public class AdrenalineRush : Ability
{
  [SerializeField]
  GameObject prefab;

  [SerializeField]
  float duration;

  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print(skillName);
      StartCoroutine(BuffStats());
      Destroy(Instantiate(prefab, transform, false), duration);
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
    yield return new WaitForSeconds(duration);
    playerController.playerSpeed -= val;
    abilityController.AccelerateCooldownRate(-.25f);
  }
}
