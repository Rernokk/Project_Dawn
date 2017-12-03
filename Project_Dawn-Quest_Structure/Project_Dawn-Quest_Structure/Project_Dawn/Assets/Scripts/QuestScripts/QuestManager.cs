using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  private QuestManager()
  {
    questsInGame = new List<Quest>();
    questsInGame.Add(new TutorialQuest());
    questsInGame.Add(new KillQuest());
    //questsInGame.Add();
  }
  private static QuestManager instance;

  [SerializeField]
  List<Quest> questsInGame;
  public string random;

  public static QuestManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new QuestManager();
      }
      return instance;
    }
  }

  private void Start()
  {
    Instance.PrintAllQuestNames();
  }

  public void PrintAllQuestNames()
  {
    foreach (Quest q in questsInGame)
    {
      print(q.QuestName);
    }
  }

  public void UpdateKillQuests(Monster obj)
  {
    IEnumerable<Quest> questQuery = (from quest in questsInGame where quest != null && quest.MyObjective == QuestObjective.KILL && (quest as KillQuest).IsTargetMonster(obj) select quest);
    if (questQuery != null)
    {
      foreach (Quest q in questQuery)
      {
        (q as KillQuest).ProgressObjective(obj);
      }
    }
  }
}
