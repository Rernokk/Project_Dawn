using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillObjectiveInterface
{
  Dictionary<Type, int> MonsterDictionary{
    get;
    set;
  }
  void AddEnemyToKillList(Type monster, int count);
}
