using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Loader : MonoBehaviour
{
  [SerializeField]
  Texture2D tex;
  // Use this for initialization
  void Awake()
  {
    Map_Manager.Instance.LoadMap(tex);
  }
}
