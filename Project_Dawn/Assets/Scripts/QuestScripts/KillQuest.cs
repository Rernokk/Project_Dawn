using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : Quest {
  List<Monster> TargetMonsterTypes;
	// Use this for initialization
	void Start () {
    MyObjective = QuestObjective.KILL;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
