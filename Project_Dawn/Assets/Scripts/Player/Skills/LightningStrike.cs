using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : Skill
{
  public GameObject myPrefab;
  public float range = 8f;
  public Player_Controller controller;
  public LightningStrike(Player_Controller controller, int levelReq, int manaCost, float cd, float ratio, GameObject prefab) : base(controller, manaCost, cd, levelReq)
  {
    skillName = "Lightning Strike";
    myPrefab = prefab;
    this.skillRatio = ratio;
  }

  public override void Cast(float damage = 0)
  {
    Debug.Log("Lightning Strike!");
    Vector3 targetPos = Vector3.zero;
    if (GameObject.Find("Persistants").GetComponent<PersistantVariables>().currentBinds != KeybindSettings.KEYBOARDONLY && !PersistantVariables.isControllerConnected)
    {
      RaycastHit2D info = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 10f);
      GameObject temp = Instantiate(myPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
      temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
      targetPos = info.point;
    } else {
      targetPos = player.Direction * 5 + (Vector2)player.transform.position;
      Instantiate(myPrefab, (Vector2)targetPos + (Vector2.up * 4), Quaternion.identity);
    }
    List<Monster> mob = new List<Monster>();
    mob = MonsterManager.Instance.MonstersInRange(targetPos, range);
    for (int i = 0; i < mob.Count; i++)
    {
      mob[i].Damage(skillRatio * player.Power);
    }
    mob.Clear();
    //player.StartCooldown(this);
  }
}
