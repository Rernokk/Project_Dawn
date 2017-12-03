using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuest : Quest
{
  public TutorialQuest()
  {
    QuestName = "Tutorial";
    MyObjective = QuestObjective.GATHER;
  }

  public override void ProgressObjective()
  {

  }
}
