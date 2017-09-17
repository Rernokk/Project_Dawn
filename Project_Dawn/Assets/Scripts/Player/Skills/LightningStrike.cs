using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : Skill
{
    public Monster_List_Ref MonsterRef;
    public GameObject myPrefab;
    public float cooldown = 1f, range = 1f;
    bool canBolt = true;
    public override void Init(float ratio = 1)
    {
        base.Init(ratio);
    }

    IEnumerator SkillCD()
    {
        canBolt = false;
        yield return new WaitForSeconds(cooldown);
        canBolt = true;
    }

    public override void Cast(int damage = 0)
    {
        if (canBolt)
        {
            //Cooldown Trigger Error
            //StartCoroutine(SkillCD());
            Vector3 targetPos = Vector3.zero;
            RaycastHit2D info = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 10f);
            GameObject temp = Instantiate(myPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
            targetPos = info.point;
            foreach (Monster m in MonsterRef.MonstersInRange(targetPos, range))
            {
                m.Damage(skillRatio * damage);
            }
        }
    }

}
