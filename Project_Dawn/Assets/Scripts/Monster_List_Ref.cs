using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_List_Ref : MonoBehaviour {
    [SerializeField]
    List<Monster> MonstersInMap;
	// Use this for initialization
	void Start () {
        MonstersInMap = new List<Monster>();
	}

    public void AddMonsterToList(Monster ctx)
    {
        MonstersInMap.Add(ctx);
    }

    public void CullMonster(Monster ctx)
    {
        MonstersInMap.Remove(ctx);
    }

    public List<Monster> MonstersInRange(Vector3 Position, float Distance)
    {
        List<Monster> tempList = new List<Monster>();
        foreach (Monster ctx in MonstersInMap)
        {
            if (Vector3.Distance(Position, ctx.transform.position) <= Distance)
            {
                tempList.Add(ctx);
            }
        }
        return tempList;
    }
}
