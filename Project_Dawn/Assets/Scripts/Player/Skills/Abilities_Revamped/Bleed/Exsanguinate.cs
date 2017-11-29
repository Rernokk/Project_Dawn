using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exsanguinate : Ability
{
  [SerializeField]
  float range, damageMult;
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print(skillName);
      List<Monster> affected = MonsterManager.Instance.MonstersInRange(transform.position, range);
      foreach (Monster monster in affected)
      {
        foreach (DamageOverTime dot in monster.GetDamageOverTimeByType(DamageType.BLEED))
        {
          dot.damageValue *= damageMult;
          dot.RefreshDuration();
        }
      }
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }
}
