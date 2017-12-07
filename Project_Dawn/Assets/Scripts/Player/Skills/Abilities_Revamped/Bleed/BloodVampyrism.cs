using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lightweight bleed, heals for a % of all bleeding damage on the target.
public class BloodVampyrism : Ability
{
  [SerializeField]
  float range = 1f;

  [SerializeField]
  float healRatio = 1;

  [SerializeField]
  GameObject prefab;

  public override void Activate()
  {
    if (!isOnCooldown){
      print(skillName);
      List<Monster> BleedingTargetsInRange = MonsterManager.Instance.MonstersInRange(transform.position, range);
      foreach (Monster monster in BleedingTargetsInRange){
        float val = 0;
        foreach (DamageOverTime dot in monster.GetDamageOverTimeByType(DamageType.BLEED)){
          val += dot.damageValue * dot.stacks;
        }
        print(monster.transform.name + " is bleeding for " + val);
      }
      isOnCooldown = true;
      prefab.transform.Find("Particle System").GetComponent<ParticleSystem>().Play();
    }
  }

  public override void Initialize()
  {
    throw new System.NotImplementedException();
  }

  void Start()
  {
    prefab = Instantiate(prefab, transform, false);
  }

}
