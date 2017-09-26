using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmp : Skill
{
  public DamageAmp(Player_Controller controller, int manaCost, int levelReq, float cd) : base(controller, manaCost, cd, levelReq)
  {
    skillName = "Damage Amp";
  }
  public override void Cast(float damage = 0)
  {
    player.AddBuff(PowerBuff(), skillName);
  }

  public IEnumerator PowerBuff(){
    player.PowerMult += .4f;
    yield return new WaitForSeconds(4f);
    player.PowerMult -= .4f;
    player.BuffNames.Remove(skillName);
    player.uiController.UpdateStats();
  }
}
