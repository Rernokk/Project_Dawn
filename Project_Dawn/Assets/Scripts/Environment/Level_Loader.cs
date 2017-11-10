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
  Texture2D tex;

  [SerializeField]
  List<ColorPair> pairs;

  [SerializeField]
  public List<BitmaskPairValue> IndexLookup = new List<BitmaskPairValue>();
  // Use this for initialization
  void Start()
  {
    Map_Manager.Instance.LoadMap(tex, pairs);
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
