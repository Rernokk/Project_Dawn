using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

  public abstract void Initialize();
  public abstract void Activate();
}
