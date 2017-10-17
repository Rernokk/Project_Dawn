using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDetection : MonoBehaviour {
  CanvasGroup KeyboardText, ControllerText;
  PersistantVariables vars;
	// Use this for initialization
	void Start () {
    KeyboardText = transform.Find("Keyboard").GetComponent<CanvasGroup>();
    ControllerText = transform.Find("Controller").GetComponent<CanvasGroup>();
    vars = GameObject.Find("Variables").GetComponent<PersistantVariables>();
    StartCoroutine(CheckForInputShift());
	}

  IEnumerator CheckForInputShift(){
    if (Input.GetJoystickNames()[0] != "")
    {
      KeyboardText.alpha = 0;
      ControllerText.alpha = 1;
      vars.isControllerConnected = true;
    } else {
      KeyboardText.alpha = 1;
      ControllerText.alpha = 0;
      vars.isControllerConnected = false;
    }
    yield return new WaitForSeconds(3f);
    StartCoroutine(CheckForInputShift());
  }
}
