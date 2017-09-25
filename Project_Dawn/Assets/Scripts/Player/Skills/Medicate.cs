using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicate : Skill
{
  float ratio;
  public Medicate (Player_Controller controller, float ratio, float cd) : base (controller, cd)
  {
    skillName = "Medicate";
    this.ratio = ratio;
  }
  
  public override void Cast(float damage = 0)
  {
    player.Heal(ratio * player.Power);
  }
}
