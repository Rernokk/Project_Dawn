using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
  [SerializeField]
  Texture2D lightningMap;
  int targetRow = 0;
  List<float> values;

  private void Start()
  {
    Init();
  }
  public void Init(float ratio = 1)
  {
    targetRow = UnityEngine.Random.Range(0, lightningMap.width);
    values = new List<float>();

    for (int i = 0; i < lightningMap.width; i++)
    {
      values.Add((2 * lightningMap.GetPixel(i, targetRow).r) - 1);
    }
    Cast();
  }

  public void Cast(int damage = 0)
  {
    StartCoroutine(ShiftPosition());
  }

  IEnumerator ShiftPosition()
  {
    while (values.Count > 0)
    {
      //int ind = UnityEngine.Random.Range(0, values.Count);
      float val = values[0];
      values.RemoveAt(0);
      transform.position += new Vector3(10 * Time.deltaTime, val / 2, 0);
      yield return null;
    }
  }
}
