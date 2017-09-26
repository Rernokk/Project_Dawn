using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : Skill
{
  public Stealth(Player_Controller controller, float cd) : base(controller, cd)
  {
    skillName = "Stealth";
  }
  public override void Cast(float damage = 0)
  {

  }
}
