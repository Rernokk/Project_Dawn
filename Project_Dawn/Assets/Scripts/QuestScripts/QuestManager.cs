using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
  private static QuestManager instance;

  [SerializeField]
  static List<Quest> questsInGame;
  public GameObject QuestEntryPrefab;

  int index = 0;

  private QuestManager()
  {
    questsInGame = new List<Quest>();
    questsInGame.Add(new TutorialQuest());
    questsInGame.Add(new ReachObjectivePoints());
    questsInGame.Add(new KillTargetDummies());

    if (QuestEntryPrefab != null)
    {
      foreach (Quest q in questsInGame)
      {
        q.myHUDEntry = QuestEntryPrefab;
      }
    }
  }

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

  public void PrintAllQuestNames()
  {
    foreach (Quest q in questsInGame)
    {
      print(q.QuestName);
    }
  }

  public void UpdateKillQuests(Monster obj)
  {
    foreach(Quest q in questsInGame){
      if (q.MyObjective == QuestObjective.KILL && q.IsUnlocked){
        print("Hitting kill quest update line for " + q.QuestName);
        (q as KillTargetDummies).UpdateProgress(obj);
      }
    }
  }

  public void UnlockQuest(string questName)
  {
    foreach (Quest q in questsInGame)
    {
      if (q.QuestName == questName)
      {
        q.UnlockQuest();
        q.Initialize();
        return;
      }
    }
  }

  public void UnlockQuest(int ind)
  {
    questsInGame[ind].UnlockQuest();
  }

  public Quest FetchQuestByName(string str)
  {
    foreach (Quest q in questsInGame)
    {
      if (q.QuestName == str)
      {
        return q;
      }
    }
    return null;
  }

  public void RemoveQuest(Quest q)
  {
    questsInGame.Remove(q);
  }

  #region Unity Methods
  private void Awake()
  {
    instance = this;
  }

  private void Start()
  {

  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha9) && false)
    {
      UnlockQuest(index);
      index++;
    }

    foreach (Quest q in questsInGame)
    {
      if (q.IsUnlocked)
      {
        q.Update();
      }
    }
  }
  #endregion
}
