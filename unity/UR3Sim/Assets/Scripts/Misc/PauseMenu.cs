using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject FPSController;

    private FirstPersonLook firstPersonLook;

    private void Start() {
        firstPersonLook = FPSController.GetComponentInChildren<FirstPersonLook>();
        Cursor.visible = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonLook.enabled = true;
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        firstPersonLook.enabled = false;
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }

    public void QuitSim()
    {
        Debug.Log("Application is quitting ...");
        Application.Quit();
    }
}