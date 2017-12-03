using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTargetDummies : Quest, IKillObjectiveInterface
{
  Dictionary<Type, int> targetPairs;
  Dictionary<Type, int> References;
  TargetDummy dummyRef;

  public KillTargetDummies()
  {
    QuestName = "Kill target dummies!";
    QuestDescription = "Earn experience and level up by fighting enemies!";
    IsUnlocked = false;
    dummyRef = new TargetDummy();
    AddEnemyToKillList(dummyRef.GetType(), 4);
    References = new Dictionary<Type, int>(targetPairs);
    MyObjective = QuestObjective.KILL;
  }

  public override void Initialize(){
    progressText.text = (References[dummyRef.GetType()] - targetPairs[dummyRef.GetType()]) + "/" + References[dummyRef.GetType()];
  }

  public Dictionary<Type, int> MonsterDictionary
  {
    get
    {
      return targetPairs;
    }

    set
    {
      targetPairs = value;
    }
  }

  public void AddEnemyToKillList(Type monster, int count)
  {
    if (targetPairs == null)
      targetPairs = new Dictionary<Type, int>();

    targetPairs.Add(monster, count);
  }

  public override void Update()
  {
    base.Update();
  }

  public override void UpdateProgress()
  {
    base.UpdateProgress();
  }

  public void UpdateProgress(Monster m){
    Type[] monsterList = new Type[targetPairs.Count];
    targetPairs.Keys.CopyTo(monsterList, 0);
    for (int i = 0; i < targetPairs.Count; i++){
      if (targetPairs.ContainsKey(m.GetType()) && targetPairs[monsterList[i]] > 0)
      {
        targetPairs[monsterList[i]]--;
        progressText.text = (References[dummyRef.GetType()] - targetPairs[dummyRef.GetType()]) + "/" + References[monsterList[i]];
        break;
      }
    }
    

    foreach (Type monster in targetPairs.Keys){
      if (targetPairs[monster] > 0){
        return;
      }
    }
    
    QuestComplete();
  }
}
