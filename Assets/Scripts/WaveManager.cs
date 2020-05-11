using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] Waves;
    private int WaveIndex = 0;

    void Update()
    {
        if (Waves[WaveIndex].transform.childCount == 0 && WaveIndex < Waves.Length - 1)
        {
            Destroy(Waves[WaveIndex]);
            WaveIndex++;
            Waves[WaveIndex].SetActive(true);
        }
        else if (Waves[WaveIndex].transform.childCount == 0)
        {
            Destroy(Waves[WaveIndex]);
        }
    }
}
