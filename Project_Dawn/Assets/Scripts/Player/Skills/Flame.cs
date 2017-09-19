using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
    public GameObject myPrefab;
    public Transform myTransform;
    public List<Monster> targets;

    public override void Init(float ratio)
    {
        myTransform = Instantiate(myPrefab, GameObject.Find("Player").transform, false).transform;
        GameObject.Find("Player").GetComponent<Player_Controller>().immobile = true;
        targets = new List<Monster>();
        base.skillRatio = ratio;
    }
    public override void Cast(int damage = 0)
    {
        Vector3 myTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        myTarget.z = myTransform.position.z;
        myTransform.LookAt(myTarget);
        myTransform.Find("TriggerZone").GetComponent<Flame_Script>().DamageTargets(damage * skillRatio);
    }

    public void DamageTargets(float dmg)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Monster tar = targets[i].GetComponent<Monster>();
            tar.Damage(dmg * Time.deltaTime);
            tar.StartCoroutine(tar.DoT(dmg / 20));
        }
    }
}
