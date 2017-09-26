using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmp : Skill
{
  public DamageAmp(Player_Controller controller) : base(controller)
  {
    skillName = "Damage Amp";
  }
  public override void Cast(float damage = 0)
  {
    
    player.PowerMult = 1.2f;
  }

  public void ResetBuff(){
    player.PowerMult = 1f;
  }
}
