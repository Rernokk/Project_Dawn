using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Skill_Bar : MonoBehaviour {
  Vector2 origin;

  [SerializeField]
  float offsetMagnitude;
  private void Start()
  {
    origin = transform.position;
  }

  public void ShiftPosition(float offset){
    transform.position = origin + (offsetMagnitude * offset * Vector2.up);
  }
}
