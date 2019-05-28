using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject ControlMenu;
    public GameObject DebugMenu;
    public GameObject Menu;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ControllsMenu()
    {
        if (DebugMenu != null) { DebugMenu.SetActive(false); }
        if (OptionsMenu != null) { OptionsMenu.SetActive(false); }
        if (ControlMenu != null) { ControlMenu.SetActive(true); }
        if (PauseMenu != null) { PauseMenu.SetActive(false); }
        Cursor.visible = true; 
        Time.timeScale = 0f;
        ClickSound();
    }
    public void Debug()
    {
        if (DebugMenu != null) { DebugMenu.SetActive(true); }
        if (OptionsMenu != null) { OptionsMenu.SetActive(false); }
        if (ControlMenu != null) { ControlMenu.SetActive(false); }
        if (PauseMenu != null) { PauseMenu.SetActive(false); }
        Cursor.visible = true;
        Time.timeScale = 0f;
        ClickSound();
    }
    public void Resume()
    {
        if (DebugMenu != null) { DebugMenu.SetActive(false); }
        if (OptionsMenu != null) { OptionsMenu.SetActive(false); }
        if (ControlMenu != null) { ControlMenu.SetActive(false); }
        if (PauseMenu != null) { PauseMenu.SetActive(false); }
        Cursor.visible = false;
        Time.timeScale = 1f;
        ClickSound();
    }
    public void MMenu()
    {
        if (DebugMenu != null) { DebugMenu.SetActive(false); }
        if (OptionsMenu != null) { OptionsMenu.SetActive(false); }
        if (ControlMenu != null) { ControlMenu.SetActive(false); }
        if (PauseMenu != null) { PauseMenu.SetActive(false); }
        if (Menu != null) { Menu.SetActive(true); }
        Cursor.visible = true;
        Time.timeScale = 1f;
        ClickSound();
    }
    public void Backbutton()
    {
        if (DebugMenu != null) { DebugMenu.SetActive(false); }
        if (OptionsMenu != null) { OptionsMenu.SetActive(false); }
        if (ControlMenu != null) { ControlMenu.SetActive(false); }
        if (PauseMenu != null) { PauseMenu.SetActive(true); }
    }
    private void ClickSound()
    {
        if (GetComponentInParent<MainMenu>() != null)
        {
            GetComponentInParent<MainMenu>().ClickSound();
        }
        else
        {
            if (FindObjectOfType<MainMenu>() != null)
            {
                FindObjectOfType<MainMenu>().ClickSound();
            }     
        }
    }
}
