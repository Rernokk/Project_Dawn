using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_Script : MonoBehaviour {
    public List<Monster> targets;
    
    private void Start()
    {
        targets = new List<Monster>();
    }

    public void DamageTargets(float dmg)
    {
        foreach (Monster target in targets)
        {
            Monster thisMonster = target.GetComponent<Monster>();
            thisMonster.Damage(dmg*Time.deltaTime);
            target.StartCoroutine(thisMonster.DoT(dmg/20));
        }
    }
}
