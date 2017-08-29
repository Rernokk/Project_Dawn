using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_System : MonoBehaviour {
    bool H = false;
    public IEnumerator Combo(KeyCode[] keys, float[] delay)
    {
        int ind = 0;
        while (!H)
        {
            yield return new WaitForSeconds(delay[ind]);
        }
        yield return null;
    }
}
