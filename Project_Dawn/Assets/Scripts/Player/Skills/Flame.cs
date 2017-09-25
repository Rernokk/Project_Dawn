using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
  public GameObject myPrefab;
  public Transform myTransform;
  public List<Monster> targets;
  public Flame (Player_Controller controller) : base(controller)
  {
    skillName = "Flame";
  }
  public override void Cast(float damage = 0)
  {

  }

  public void DamageTargets(float dmg)
  {
    for (int i = 0; i < targets.Count; i++)
    {
      Monster tar = targets[i].GetComponent<Monster>();
      tar.Damage(dmg * Time.deltaTime);
      tar.StartCoroutine(tar.DoT(dmg / 20));
    }
  }
}
