using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Select : MonoBehaviour {
  List<Button> buttons;
  CanvasGroup group;
	// Use this for initialization
	void Start () {
    buttons = new List<Button>();
    foreach (Transform t in transform){
      if (t.GetComponent<Button>()){
        buttons.Add(t.GetComponent<Button>());
      }
    }
    group = GetComponent<CanvasGroup>();
    buttons[0].Select();
	}

  public void Sel()
  {
    if (buttons != null && buttons.Count > 0)
    {
      buttons[0].Select();
    }
  }
}
