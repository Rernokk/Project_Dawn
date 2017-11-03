using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bitmasking_Value : MonoBehaviour
{
  [Serializable]
  public struct BitmaskPairValue {
    public int index;
    public int spriteIndex;

    public BitmaskPairValue (int a, int b) {
      index = a;
      spriteIndex = b;
    }
  }

  [SerializeField]
  Sprite[] tileSet;

  [SerializeField]
  public List<BitmaskPairValue> IndexLookup = new List<BitmaskPairValue>();

  [SerializeField]
  int bitmaskValue = 0;
  int[] neighborArray;

  private void Start()
  {
    bitmaskValue = 0;
    neighborArray = new int[8];
    Stall();
  }

  void Stall()
  {
    //yield return null;
    int i = 0;
    for (int j = -1; j <= 1; j++)
    {
      for (int k = -1; k <= 1; k++)
      {
        if (j == 0 && k == 0)
        {
          continue;
        }
        else
        {
          Vector3 checkPos = transform.position + new Vector3(j, k, 0);
          neighborArray[i] = Map_Manager.Instance.IsTileOpen(checkPos);
          i++;
        }
      }
    }
    for (int j = 0; j < 8; j++)
    {
      bitmaskValue += (int) Mathf.Pow(2, j) * neighborArray[j];
    }
    foreach (BitmaskPairValue pair in IndexLookup){
      if (pair.spriteIndex == bitmaskValue){
        GetComponent<SpriteRenderer>().sprite = tileSet[pair.index];
      }
    }
  }
}
