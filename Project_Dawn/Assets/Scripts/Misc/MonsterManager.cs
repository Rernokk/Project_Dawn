using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
  [SerializeField]
  private static List<Monster> MonstersInMap = new List<Monster>();

  private static MonsterManager instance;

  private MonsterManager()
  {
    //MonstersInMap = new List<Monster>();
  }
  public static MonsterManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new MonsterManager();
      }
      return instance;
    }
  }

  public void AddMonsterToList(Monster ctx)
  {
    MonstersInMap.Add(ctx);
  }

  public void CullMonster(Monster ctx)
  {
    MonstersInMap.Remove(ctx);
  }

  public List<Monster> MonstersInRange(Vector2 Position, float Distance)
  {
    List<Monster> tempList = new List<Monster>();
    foreach (Monster ctx in MonstersInMap)
    {
      if (Vector2.Distance(Position, ctx.transform.position) <= Distance)
      {
        tempList.Add(ctx);
      }
    }
    return tempList;
  }

  public void ListMonstersOut(){
    foreach (Monster m in MonstersInMap){
      Debug.Log(m.transform.name);
    }
  }
}
