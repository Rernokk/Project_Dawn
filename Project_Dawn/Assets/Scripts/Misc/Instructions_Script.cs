using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions_Script : MonoBehaviour
{
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton1))
    {
      SceneManager.LoadScene("Level-0");
    }
    if (Input.GetKeyDown(KeyCode.JoystickButton9)){
      SceneManager.LoadScene("Instructions");
    }
    if (Input.GetKeyDown(KeyCode.JoystickButton10)){
      SceneManager.LoadScene("Credits");
    }
  }
}
