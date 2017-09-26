using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicate : Skill
{
  float ratio;
  public GameObject myPrefab;
  public Medicate (Player_Controller controller, int manaCost, float ratio, float cd, GameObject fx) : base(controller, manaCost, cd)
  {
    skillName = "Medicate";
    this.ratio = ratio;
    myPrefab = fx;
  }
  
  public override void Cast(float damage = 0)
  {
    player.Heal(ratio * player.Power);
    Destroy(Instantiate(myPrefab, player.transform, false), 4f);
  }
}
