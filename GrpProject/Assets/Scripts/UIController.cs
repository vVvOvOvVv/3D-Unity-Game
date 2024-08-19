using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] Image pauseMenu;
    [SerializeField] Image restartPrompt;
    [SerializeField] Image quitPrompt;
    GameObject player;
    GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //get references to the player and camera
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //display the cursor and pause menu when ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOpenPauseMenu();
        }
    }

    public void OnClosePauseMenu()
    {
        //don't display pause menu
        pauseMenu.gameObject.SetActive(false);
        player.GetComponent<FPSInput>().enabled = true;
        player.GetComponent<MouseLook>().enabled = true;
        mainCamera.GetComponent<MouseLook>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void OnOpenPauseMenu()
    {
        //display pause menu
        pauseMenu.gameObject.SetActive(true);
        player.GetComponent<FPSInput>().enabled = false;
        player.GetComponent<MouseLook>().enabled = false;
        mainCamera.GetComponent<MouseLook>().enabled = false;

        //unlock and display cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void OnOpenRestartPrompt()
    {
        restartPrompt.gameObject.SetActive(true);
    }

    public void OnCloseRestartPrompt()
    {
        restartPrompt.gameObject.SetActive(false);
    }

    public void OnOpenQuitPrompt()
    {
        quitPrompt.gameObject.SetActive(true);
    }

    public void OnCloseQuitPrompt()
    {
        quitPrompt.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}