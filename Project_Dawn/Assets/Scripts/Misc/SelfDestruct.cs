using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
    [SerializeField]
    float destroyTime;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime);
	}
}
