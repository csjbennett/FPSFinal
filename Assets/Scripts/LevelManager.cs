using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].gameObject.GetComponent<Enemy>().PlayerSeen = true;
            }
        }
    }
}
