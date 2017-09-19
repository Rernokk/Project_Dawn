using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : Skill
{
    public Monster_List_Ref MonsterRef;
    public GameObject myPrefab;
    public float range = 1f;

    public float CD
    {
        get
        {
            return cooldown;

        }

        set
        {
            cooldown = value;
        }
    }

    public override void Init(float ratio = 1)
    {
        base.Init(ratio);
    }

    public override void Cast(int damage = 0)
    {
        if (cooled)
        {
            //Cooldown Trigger Error
            //StartCoroutine(SkillCD());
            Vector3 targetPos = Vector3.zero;
            RaycastHit2D info = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 10f);
            GameObject temp = Instantiate(myPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
            targetPos = info.point;
            List<Monster> mob = MonsterRef.MonstersInRange(targetPos, range);
            foreach (Monster m in mob)
            {
                m.Damage(skillRatio * damage);
            }
            mob.Clear();
        }
    }

}
