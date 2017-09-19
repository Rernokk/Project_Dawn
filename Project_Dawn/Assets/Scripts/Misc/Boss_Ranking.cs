using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Ranking : MonoBehaviour
{
    [SerializeField]
    Sprite[] scoreArray;
    Image renderer;
    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Image>();
        renderer.color = Color.clear;
    }

    public void DetermineScore(float time)
    {
        if (time < 10)
        {
            renderer.sprite = scoreArray[7];
        }
        else if (time < 20)
        {
            renderer.sprite = scoreArray[6];
        }
        else if (time < 30)
        {
            renderer.sprite = scoreArray[5];
        }
        else if (time < 40)
        {
            renderer.sprite = scoreArray[4];
        }
        else if (time < 60)
        {
            renderer.sprite = scoreArray[3];
        }
        else if (time < 120)
        {
            renderer.sprite = scoreArray[2];
        }
        else if (time < 180)
        {
            renderer.sprite = scoreArray[1];
        }
        renderer.color = Color.white;
        StartCoroutine(countDown());
    }


    IEnumerator countDown() {
        yield return new WaitForSeconds(5f);
        renderer.color = Color.clear;
    }

    public void YouFailed() {
        renderer.color = Color.white;
        renderer.sprite = scoreArray[0];
    }
}
