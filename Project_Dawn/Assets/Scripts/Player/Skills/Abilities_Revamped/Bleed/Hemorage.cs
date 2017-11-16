using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Apply a bleeding effect to the first target hit.
public class Hemorage : Ability
{
  [SerializeField]
  GameObject hemoragePrefab;

  [SerializeField]
  float damageRatio;

  [SerializeField]
  int maxBleedStack;

  [SerializeField]
  float dotDuration;

  public override void Initialize()
  {

  }

  public override void Activate()
  {
    if (!isOnCooldown)
    {
      if (abilityController.Target == null)
      {
        abilityController.SelectTarget();
      }

      if (abilityController.Target == null)
      {
        Vector3 dir = VectorToMouse();
        GameObject projectile = Instantiate(hemoragePrefab, (1.25f * dir) + transform.position + (Vector3.up * .5f), Quaternion.identity);
        Hemorage_Projectile_Script referenceScript = projectile.GetComponent<Hemorage_Projectile_Script>();
        referenceScript.dir = dir;
        referenceScript.damageValue = Power * damageRatio;
        referenceScript.MaxStack = maxBleedStack;
        referenceScript.duration = dotDuration;
      }
      else
      {
        Vector3 dir = VectorToTarget();
        GameObject projectile = Instantiate(hemoragePrefab, (1.25f * dir) + transform.position + (Vector3.up * .5f), Quaternion.identity);
        Hemorage_Projectile_Script referenceScript = projectile.GetComponent<Hemorage_Projectile_Script>();
        referenceScript.dir = dir;
        referenceScript.damageValue = Power * damageRatio;
        referenceScript.MaxStack = maxBleedStack;
        referenceScript.duration = dotDuration;
      }
      print("Hemorage");
      isOnCooldown = true;
    }
  }
}
