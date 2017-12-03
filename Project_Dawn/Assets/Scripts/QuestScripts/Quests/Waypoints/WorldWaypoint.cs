using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum QuestType { WORLD, OTHER };
public class WorldWaypoint : MonoBehaviour
{
  Quest myQuestComponent;

  [SerializeField]
  string QuestName;

  [SerializeField]
  QuestType myQuestType = QuestType.WORLD;
  bool hasHitPlayer = false;

  // Use this for initialization
  void Start()
  {
    myQuestComponent = QuestManager.Instance.FetchQuestByName(QuestName);
    if (myQuestType == QuestType.WORLD)
    {
      (myQuestComponent as ReachObjectivePoints).AddWaypoint(gameObject);
    }
  }

  private void Update()
  {
    if (hasHitPlayer)
    {
      hasHitPlayer = false;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.tag == "Player" && !hasHitPlayer)
    {
      hasHitPlayer = true;
      switch (myQuestType)
      {
        case (QuestType.WORLD):
          (myQuestComponent as ReachObjectivePoints).UpdateProgress(this);

          break;
      }
    }
  }

  public Quest QuestComponent
  {
    get
    {
      return myQuestComponent;
    }

    set
    {
      myQuestComponent = value;
    }
  }
}
