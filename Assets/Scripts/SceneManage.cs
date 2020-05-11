using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public GameObject Enemies;
    public bool Finished;
    public bool Failed;
    public RawImage FadeOut;
    private float FadeA;
    private GameObject Player;
    private int CurrentScene;

    private void Start()
    {
        Finished = false;
        FadeA = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (Enemies.transform.childCount == 0 && !Finished && !Failed)
        {
            Finished = true;
            Player.GetComponent<PlayerScript>().Finish = true;
        }

        if (Finished)
        {
            FadeOut.color = new Color(0, 0, 0, FadeA);
            FadeA += (Time.deltaTime * 0.25f);
        }

        if (Failed && !Finished)
        {
            FadeOut.color = new Color(Mathf.Lerp(FadeOut.color.r, 0, 0.2f), 0, 0, FadeA);
            FadeA += Time.deltaTime;
        }

        if (FadeA > 1.5f && Finished)
        {
            SceneManager.LoadScene(CurrentScene + 1);
        }

        if (FadeA > 1.5f && Failed)
        {
            SceneManager.LoadScene(CurrentScene);
        }
    }

    public void Failure()
    {
        Failed = true;
    }
}
