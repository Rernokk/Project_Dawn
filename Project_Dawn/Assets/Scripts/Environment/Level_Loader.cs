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
<<<<<<< HEAD
  List<ColorPair> pairs;

  [SerializeField]
  public List<BitmaskPairValue> IndexLookup = new List<BitmaskPairValue>();
=======
  List<ColorPair> bgPairs, levelPairs, fgPairs;
>>>>>>> 8cb503434ab7598fc460e22d2860e2092b5d9cf1
  // Use this for initialization
  void Start()
  {
    Map_Manager.Instance.LoadMap(fgTex, fgPairs, 0);
    Map_Manager.Instance.LoadMap(levelTex, levelPairs, 1);
    Map_Manager.Instance.LoadMap(bgTex, bgPairs, 2);
  }

  public int FetchSpriteIndex(int i){
    foreach (BitmaskPairValue t in IndexLookup){
      if (i == t.spriteIndex){
        return t.index;
      }
    }
    return 47;
  }
}
