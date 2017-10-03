using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
  public GameObject myPrefab;
  public List<Monster> targets;
  public Flame (Player_Controller controller, GameObject prefab, float ratio, int manaCost, float cd) : base(controller, manaCost, cd)
  {
    skillName = "Fireball";
    myPrefab = prefab;
    skillRatio = ratio;
  }
  public override void Cast(float damage = 0)
  {
    GameObject temp = Instantiate(myPrefab, (Vector2)player.transform.position + -player.dir + (Vector2.up * .5f), Quaternion.identity);
    temp.GetComponent<Fireball_Projectile>().dmg = skillRatio * damage;
    temp.GetComponent<Fireball_Projectile>().player = player;
    Destroy(temp, 4f);
    player.StartCooldown(this);
  }
}
