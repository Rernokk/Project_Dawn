using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachObjectivePoints : Quest {
  List<GameObject> waypoints;
  int currentCount = 0, totalCount = 0;
  bool hasHitThisFrame = false;

	public ReachObjectivePoints () {
    QuestName = "Explore the level!";
    QuestDescription = "Explore the level by reaching goals in the level!";
    IsUnlocked = false;
    MyObjective = QuestObjective.REACH;
	}

  public void AddWaypoint(GameObject obj){
    if (waypoints == null){
      waypoints = new List<GameObject>();
    }
    obj.GetComponent<WorldWaypoint>().QuestComponent = this;
    waypoints.Add(obj);
    totalCount++;
    if (progressText != null)
    {
      progressText.text = currentCount + "/" + waypoints.Count;
    }
  }

  public override void UpdateProgress()
  {
    base.UpdateProgress();
  }

  public void UpdateProgress(WorldWaypoint pt){
    if (!IsUnlocked)
    { 
      return;
    }

    waypoints.Remove(pt.gameObject);
    if (waypoints.Count > 0)
    {
      print("Registered progress, " + waypoints.Count + " remaining");
      currentCount++;
      progressText.text = currentCount + "/" + totalCount;
    } else {
      print("Quest Complete!");
      QuestComplete();
      IsComplete = true;
    }
    Destroy(pt.gameObject);
  }

  public override void UnlockQuest()
  {
    base.UnlockQuest();
    totalCount = waypoints.Count;
    progressText.text = currentCount + "/" + totalCount;
  }

  public override void Update()
  {
    base.Update();
  }
}
