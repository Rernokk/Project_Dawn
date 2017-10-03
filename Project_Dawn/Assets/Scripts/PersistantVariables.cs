using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum KeybindSettings { NORMAL, KEYBOARDONLY, SOUTHPAW };
public class PersistantVariables : MonoBehaviour {
  public KeybindSettings currentBinds = KeybindSettings.NORMAL;
  public Transform myDropdown;
	// Use this for initialization
	void Start () {
    DontDestroyOnLoad(gameObject);
    myDropdown = transform.Find("Dropdown");
	}
	
	// Update is called once per frame
	void Update () {
    if (SceneManager.GetActiveScene().name == "Instructions")
    {
      if (!GameObject.Find("Canvas").transform.Find("Dropdown(Clone)"))
      {
        Transform temp = Instantiate(myDropdown, GameObject.Find("Canvas").transform, false);
        temp.GetComponent<CanvasGroup>().alpha = 1;
        temp.GetComponent<Dropdown>().value = (int) currentBinds;
      }
    }
	}

  public void SetKeybindSet(int ctx){
    currentBinds = (KeybindSettings)ctx;
  }
}
