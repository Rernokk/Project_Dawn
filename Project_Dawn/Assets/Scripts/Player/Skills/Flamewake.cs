using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamewake : Skill
{
  GameObject projectile;
  float moveSpeed;
  public Flamewake(Player_Controller controller, int manaCost, GameObject prefab, float ratio, int levelReq, float cd = 1, float speed = 1f) : base(controller, manaCost, cd, levelReq)
  {
    projectile = prefab;
    moveSpeed = speed;
    skillRatio = ratio;
  }

  public override void Cast(float damage = 0)
  {
    GameObject temp = Instantiate(projectile, (Vector2)player.transform.position + -player.Direction, Quaternion.identity);
    temp.GetComponent<Flamewake_Projectile>().speed = moveSpeed;
    temp.GetComponent<Flamewake_Projectile>().dir = -player.Direction;
    temp.GetComponent<Flamewake_Projectile>().damage = damage * skillRatio;
    Destroy(temp, 3f);
  }
}
