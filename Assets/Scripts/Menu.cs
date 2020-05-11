using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public int Scene1;
    public int Scene2;
    public int Scene3;

    public void LoadScene1()
    {
        SceneManager.LoadScene(Scene1);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(Scene2);
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene(Scene3);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
