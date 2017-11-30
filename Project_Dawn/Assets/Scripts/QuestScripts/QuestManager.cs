using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  private QuestManager() {
    questsInGame = new List<Quest>();
    questsInGame.Add(new TutorialQuest());
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

  public void PrintAllQuestNames(){
    foreach (Quest q in questsInGame){
      print(q.QuestName);
    }
  }

  public void UpdateKillQuests(Monster obj){
    print(obj.ToString());
  }
}
