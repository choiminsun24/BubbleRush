using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEffect : MonoBehaviour
{
    public int[] keys;
    public GameObject[] values;

    Dictionary<int, GameObject> data;

    void Awake()
    {

        for (int i = 0; i < keys.Length; i++) //Dictionary ¼¼ÆÃ
        {
            data.Add(keys[i], values[i]);
        }
    }

    public void callEffect(int num, Transform tf)
    {
        
    }
}
