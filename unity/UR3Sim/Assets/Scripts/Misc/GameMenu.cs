using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenu : MonoBehaviour
{
    public GameObject inputField;
    private void Awake() {
        string lastIP = PlayerPrefs.GetString("ROSIP", "192.168.2.2");
        inputField.GetComponent<TMP_InputField>().text = lastIP;
    }

    public void StartSim()
    {
        SceneManager.LoadScene("UR3 World");
    }

    public void QuitSim()
    {
        Debug.Log("Application is quitting ...");
        Application.Quit();
    }

    public void ChangeIP()
    {
        string ip = inputField.GetComponent<TMP_InputField>().text;
        PlayerPrefs.SetString("ROSIP", ip);
    }
}