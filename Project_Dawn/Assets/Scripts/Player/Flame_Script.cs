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
        for (int i = 0; i < targets.Count; i++)
        {
            Monster tar = targets[i].GetComponent<Monster>();
            tar.Damage(dmg * Time.deltaTime);
        }
    }
}
