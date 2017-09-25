using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Skill
{
  public Teleport (Player_Controller controller, float cd) : base(controller, cd)
  {
    skillName = "Teleport";
  }
  public override void Cast(float damage = 0)
  {
  }

}
