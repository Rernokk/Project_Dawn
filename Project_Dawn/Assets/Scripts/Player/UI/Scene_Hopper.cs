using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Hopper : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Return))
    {
      SceneManager.LoadScene("Title");
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      SceneManager.LoadScene("Playground");
    }

    if (Input.GetKeyDown(KeyCode.C))
    {
      SceneManager.LoadScene("Credits");
    }

    if (Input.GetKeyDown(KeyCode.Escape)){
      SceneManager.LoadScene("Instructions");
    }

    if (Input.GetKeyDown(KeyCode.Q)){
      print("Quitting");
      Application.Quit();
    }
  }
}
