using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuest : Quest
{
  string[] StageText;
  int stage = -1;
  bool movedLeft = false, movedRight = false;
  public TutorialQuest()
  {
    StageText = new string[] { "Press A to move left and D to move right.", "Press shift and left-click an enemy to target them.", "Press left-click to attack!" };
    QuestName = "Tutorial";
    QuestDescription = "This quest is meant to help you learn controls! Press Space to begin!";
    IsUnlocked = false;
    IsComplete = false;
  }

  public override void Update()
  {
    base.Update();

    if (stage < StageText.Length)
    {
      int preStage = stage;

      //Stage 2, Attacking
      if (stage == 2)
      {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          stage++;
        }
      }

      //Stage 1, Targets
      if (stage == 1)
      {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0) && GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Abilities_Controller>().Target != null)
        {
          stage++;
        }
      }

      //Stage 0, Movement
      if (stage == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
      {
        if (Input.GetKeyDown(KeyCode.A))
        {
          movedLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
          movedRight = true;
        }

        if (movedLeft && movedRight)
        {
          stage++;
        }
      }

      //Stage Start, Jumping
      if (stage == -1 && Input.GetKeyDown(KeyCode.Space)){
        stage++;
      }

      if (preStage != stage && stage < StageText.Length)
      {
        descriptionText.text = StageText[stage];
      }
    }

    if (stage == StageText.Length && !IsComplete)
    {
      QuestComplete();
      QuestManager.Instance.UnlockQuest("Kill target dummies!");
      QuestManager.Instance.UnlockQuest("Explore the level!");
    }
  }

  public override void UnlockQuest()
  {
    base.UnlockQuest();
  }
}
