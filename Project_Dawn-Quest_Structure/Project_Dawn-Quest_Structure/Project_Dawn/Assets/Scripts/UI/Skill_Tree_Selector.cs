using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Tree_Selector : MonoBehaviour {
  public string skill;
  Selected_Skill_Manager manager;
  Player_Abilities_Controller abilityController;
  Button button;

	void Start () {
    transform.Find("Text").GetComponent<Text>().text = skill;
    transform.Find("Text").GetComponent<Text>().fontSize = 12;
    manager = GameObject.Find("Skill_Info").GetComponent<Selected_Skill_Manager>();
    button = GetComponent<Button>();
    button.onClick.AddListener(SetSkill);
    abilityController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Abilities_Controller>();
    if (skill == ""){
      button.interactable = false;
    }
  }

  public void SetSkill(){
    manager.SelectedAbility = abilityController.GetAbility(skill);
    manager.UpdateText();
  }
}
