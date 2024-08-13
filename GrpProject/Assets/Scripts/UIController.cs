using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] Image pauseMenu;
    GameObject player;
    GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //don't display the pause menu on start
        pauseMenu.gameObject.SetActive(false);

        //get references to the player and camera
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //display the cursor when ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // unlock and display the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OnOpenSettings();
        }
    }

    public void OnCloseSettings()
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

    public void OnOpenSettings()
    {
        //display pause menu
        pauseMenu.gameObject.SetActive(true);
        player.GetComponent<FPSInput>().enabled = false;
        player.GetComponent<MouseLook>().enabled = false;
        mainCamera.GetComponent<MouseLook>().enabled = false;
        Time.timeScale = 0;
    }

}