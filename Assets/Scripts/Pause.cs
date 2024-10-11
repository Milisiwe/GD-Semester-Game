using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject Panel;

    private void Start()
    {
        Panel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");

            if (Panel.activeInHierarchy)
            {
                Panel.SetActive(false);
                Time.timeScale = 1;
            }
            
            else
            {
                Time.timeScale = 0;
                Panel.SetActive(true);
            }
        }
    }

    public void UnpausePlayGame()
    {
        Panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
