using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject BuyMenu;
    public GameObject ControlMenu;
    public GameObject DebugMenu;
    public Text objectivesList;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        if (BuyMenu != null) { BuyMenu.SetActive(false); }
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BuyMenu.activeSelf == false)
            {
                if (isPaused)
                {
                    ResumeGame();          
                }
                else
                {
                    PlayerSwitch(false);
                    isPaused = true;
                    pauseMenu.SetActive(true);
                    ObjectivesUpdate();
                    Time.timeScale = 0f;
                    if (BuyMenu != null) { BuyMenu.SetActive(false); }
                }
            }
        }
    }

    public void ResumeGame()
    {
        PlayerSwitch(true);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        isPaused = false;
        if (DebugMenu != null) { DebugMenu.SetActive(false); }
        if (optionsMenu != null) { optionsMenu.SetActive(false); }
        if (ControlMenu != null) { ControlMenu.SetActive(false); }
        if (pauseMenu != null) { pauseMenu.SetActive(false); }
        if (BuyMenu != null) { BuyMenu.SetActive(false); }
        Time.timeScale = 1f;

    }

    public void BackButton()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        if (DebugMenu != null)
        {
            DebugMenu.SetActive(false);
        }

        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }

        if (ControlMenu != null)
        {
            ControlMenu.SetActive(false);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        if (BuyMenu != null)
        {
            BuyMenu.SetActive(false);
        }

    }

    public void ControlsMenu()
    {
        if (ControlMenu != null)
        {
            ControlMenu.SetActive(true);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void PlayerSwitch(bool turnOn)
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var turnScript = player.GetComponent<AMS_Aim>();
            if (turnScript != null) { turnScript.enabled = turnOn; }
            else { Debug.Log("can't find aiming script"); }

            var gunScript = player.GetComponent<AMS_GunManagement>();
            if (gunScript != null) { gunScript.enabled = turnOn; }
            else { Debug.Log("can't find gun script"); }
        }
        else { Debug.Log("can't find player"); }

        var gunCursor = FindObjectOfType<WeaponCursorGUI>();
        if(gunCursor != null) { gunCursor.enabled = turnOn; }
        else { Debug.Log("can't find gun cursor"); }
    }

    private void ObjectivesUpdate()
    {
        if (AMS_UniversalFunctions.levelNumber != 0)
        {
            objectivesList.text = AMS_UniversalFunctions.objectivesText[AMS_UniversalFunctions.levelNumber - 1];
        }
        else
        {
            objectivesList.text = "Must Enter From Level Select to Display.";
        }

    }

}
