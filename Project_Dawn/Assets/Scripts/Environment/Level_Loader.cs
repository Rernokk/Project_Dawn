using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ColorPair
{
  public Color col;
  public GameObject obj;
}

public class Level_Loader : MonoBehaviour
{
  [SerializeField]
  Texture2D bgTex, levelTex, fgTex;

  [SerializeField]
  List<ColorPair> bgPairs, levelPairs, fgPairs;
  // Use this for initialization
  void Start()
  {
    Map_Manager.Instance.LoadMap(fgTex, fgPairs, 0);
    Map_Manager.Instance.LoadMap(levelTex, levelPairs, 1);
    Map_Manager.Instance.LoadMap(bgTex, bgPairs, 2);
  }
}
