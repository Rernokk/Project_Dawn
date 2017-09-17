using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected float skillRatio;
    public virtual void Init(float ratio = 1)
    {
        skillRatio = ratio;
    }

    public abstract void Cast(int damage = 0);
}
