using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : Object
{
  protected float skillRatio = 1f, cooldown = 1f;
  protected bool cooled = true;
  public string skillName = "";
  protected Player_Controller player;
  public Skill (Player_Controller controller, float cd = 1f){
    this.player = controller;
    cooldown = cd;
  }
  public float CooldownDuration
  {
    get { return cooldown; }
    set { cooldown = value; }
  }

  public bool IsCooledDown
  {
    get { return cooled; }
    set { cooled = value; }
  }

  public string Name
  {
    get { return skillName; }
    set { skillName = value; }
  }

  public virtual void Init(float ratio = 1)
  {
    skillRatio = ratio;
  }

  public abstract void Cast(float damage = 0);

  protected IEnumerator SkillCD()
  {
    cooled = false;
    yield return new WaitForSeconds(cooldown);
    cooled = true;
  }
}
