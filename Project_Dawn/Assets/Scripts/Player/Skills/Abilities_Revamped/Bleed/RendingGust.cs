using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Apply a bleeding effect to targets within an area.
public class RendingGust : Ability
{
  [SerializeField]
  GameObject rendingAnchorPrefab;

  [SerializeField]
  float range, duration, ratio;

  [SerializeField]
  int maxBleedStack;
  public override void Activate()
  {
    if (!isOnCooldown)
    {
      print(skillName);

      GameObject anchor;
      if (abilityController.Target == null)
      {
        anchor = Instantiate(rendingAnchorPrefab, transform.position, Quaternion.identity);
      }
      else
      {
        anchor = Instantiate(rendingAnchorPrefab, abilityController.Target.transform.position, Quaternion.identity);
      }
      List<Monster> Hits = MonsterManager.Instance.MonstersInRange(anchor.transform.position, range);

      foreach (Monster hit in Hits)
      {
        hit.AddDot("RendingGust", duration, ratio * playerController.power, 1, DamageType.BLEED, maxBleedStack);
      }
      Destroy(anchor, 2f);
      isOnCooldown = true;
    }
  }

  public override void Initialize()
  {
    
  }
}
