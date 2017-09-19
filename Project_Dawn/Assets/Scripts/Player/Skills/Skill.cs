using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected float skillRatio, cooldown = 1f;
    protected bool cooled = true;

    public virtual void Init(float ratio = 1)
    {
        skillRatio = ratio;
    }

    public abstract void Cast(int damage = 0);

    protected IEnumerator SkillCD()
    {
        cooled = false;
        yield return new WaitForSeconds(cooldown);
        cooled = true;
    }
}
