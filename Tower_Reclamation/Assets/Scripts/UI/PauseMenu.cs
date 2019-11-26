﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {


    public static bool isPaused = false;

    [SerializeField] Button save;
    [SerializeField] Button load;

    [SerializeField] GameObject pauseMenu;
    // Use this for initialization
    void Start() {
        // starts unpaused.
        Resume();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        print(SceneManager.GetActiveScene().name.ToString());
        if (!SceneManager.GetActiveScene().name.Equals("_Base"))
        {
            save.gameObject.SetActive(false);
            load.gameObject.SetActive(false);
            //load.interactable = false;
        }
        else
        {
            save.gameObject.SetActive(true);
            load.gameObject.SetActive(true);
        }
    }

    public void LoadMenu()
    {
        print("loading the menu...");
    }

    public void QuitGame()
    {
        print("Quitting the game...");
    }
}
