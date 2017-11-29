using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selected_Skill_Manager : MonoBehaviour {
  Ability selectedAbility;
  Text nameText, levelRequirementText, descriptionText;
  Button select;
  Player_Abilities_Controller abilityController;

  public Ability SelectedAbility {
    get {
      return selectedAbility;
    }

     set {
      selectedAbility = value;
     }
  }
	// Use this for initialization
	void Start () {
    nameText = transform.Find("InformationRack/Name/Text").GetComponent<Text>();
    levelRequirementText = transform.Find("InformationRack/LevelReq/Text").GetComponent<Text>();
    descriptionText = transform.Find("Description/Text").GetComponent<Text>();
    abilityController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Abilities_Controller>();
    select = transform.Find("Select_Skill").GetComponent<Button>();
  }
	
	// Update is called once per frame
	public void UpdateText () {
    if (selectedAbility == null)
      return;

    nameText.text = selectedAbility.Name;
    levelRequirementText.text = selectedAbility.RequiredLevel.ToString();
    descriptionText.text = selectedAbility.Description;
    if (selectedAbility.RequiredLevel > abilityController.GetComponent<Player_Controller>().Level)  {
      select.interactable = false;
    } else {
      select.interactable = true;
    }
  }

  public void ApplyAbility(){
    abilityController.SetAbility(selectedAbility);
  }
}
