using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestObjective { KILL, GATHER, REACH };
public abstract class Quest
{
  QuestObjective myObjective = QuestObjective.KILL;
  bool isUnlocked, isComplete, isPrimary;
  public string QuestName;

  public bool IsUnlocked
  {
    get
    {
      return isUnlocked;
    }

    set
    {
      isUnlocked = value;
    }
  }

  public bool IsComplete {
    get {
      return isComplete;
    }

    set {
      isComplete = value;
    }
  }

  public bool IsPrimary {
    get{
      return isPrimary;
    }

    set {
      isPrimary = value;
    }
  }

  public QuestObjective MyObjective{
    get {
      return myObjective;
    }

    protected set {
      myObjective = value;
    }
  }

  public abstract void ProgressObjective();
}
