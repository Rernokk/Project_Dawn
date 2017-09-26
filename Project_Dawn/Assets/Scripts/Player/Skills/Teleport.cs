using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Skill
{
  public Teleport(Player_Controller controller, int manaCost, float cd) : base(controller, manaCost, cd)
  {
    skillName = "Teleport";
  }
  public override void Cast(float damage = 0)
  {
  }

}
