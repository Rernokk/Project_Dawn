using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : Quest, KillObjectiveInterface
{
  List<Monster> TargetMonsterTypes;
  Dictionary<string, int> Objectives;
  string CurrentType;

  public KillQuest()
  {
    QuestName = "Kill Quest!";
    MyObjective = QuestObjective.KILL;
    TargetMonsterTypes = new List<Monster>();
    Objectives = new Dictionary<string, int>();
    AddEnemyToKillList(new TargetDummy(), 2);
    CurrentType = "";
  }

  public bool IsTargetMonster(Monster monster)
  {
    Debug.Log(monster.ToString());
    foreach (Monster m in TargetMonsterTypes)
    {
      if (m.GetType() == monster.GetType())
      {
        CurrentType = monster.GetType().ToString();
        return true;
      }
    }
    return false;
  }

  public void AddEnemyToKillList(Monster monster, int count)
  {
    TargetMonsterTypes.Add(monster);
    Objectives.Add(monster.GetType().ToString(), count);
  }

  public override void ProgressObjective()
  {
      
  }

  public void ProgressObjective(Monster monster)
  {
    Objectives[CurrentType]--;
    CurrentType = "";
    foreach (int val in Objectives.Values)
    {
      if (val > 0)
      {
        Debug.Log(val);
        return;
      }
    }
    IsComplete = true;
    Debug.Log("Objective Completed!");
  }
}
