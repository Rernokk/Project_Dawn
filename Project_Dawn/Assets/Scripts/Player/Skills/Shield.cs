using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Skill
{
  public Shield(Player_Controller controller, int manaCost, float cd) : base(controller, manaCost, cd)
  {
    skillName = "Shield";
  }
  public override void Cast(float damage = 0)
  {
    player.AddBuff(ShieldDuration(), skillName);
    player.StartCooldown(this);
  }

  IEnumerator ShieldDuration(){
    player.defenseMult += .5f;
    yield return new WaitForSeconds(12f);
    player.defenseMult -= .5f;
    player.BuffNames.Remove(skillName);
    player.uiController.UpdateStats();
  }
}
