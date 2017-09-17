using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
    public GameObject myPrefab;
    public Transform myTransform;
    public override void Init(float ratio)
    {
        myTransform = Instantiate(myPrefab, GameObject.Find("Player").transform, false).transform;
        base.skillRatio = ratio;
    }
    public override void Cast(int damage = 0)
    {
        Vector3 myTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        myTarget.z = myTransform.position.z;
        myTransform.LookAt(myTarget);
        myTransform.Find("TriggerZone").GetComponent<Flame_Script>().DamageTargets(damage * skillRatio);
    }
}
