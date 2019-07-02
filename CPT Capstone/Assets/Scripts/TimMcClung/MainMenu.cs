using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject playButton;
    public GameObject pauseMenu;
    public GameObject mainmenu;
    public AudioClip menusounds;
    public GameObject contButt;
    public GameObject controlsMenu;
    public int levelIndex;
    public bool loadPlayer;

    private void Update()
    {
        if (contButt != null)
        {
            var bScript = contButt.GetComponent<Button>();
            var playButtText = playButton.GetComponentInChildren<Text>(); //this should be checked for null but it's late and I'm tired -KE
            if (bScript != null)
            {
                var Loader = FindObjectOfType<NewSaveAndLoad>();
                if (Loader != null)
                {
                    var path = Loader.dataPathPos;
                    if (Loader.SaveExists(path))
                    {
                        bScript.interactable = true;
                        playButtText.text = "Play";
                    }
                    else
                    {
                        bScript.interactable = false;
                        playButtText.text = "Play";
                    }

                }
                else { Debug.Log("there should be a save and load prefab in the scene"); bScript.interactable = false; }
            }
            else { Debug.Log("component not found in button obj"); }
        }
        //else { Debug.Log("only matters if you see this in the main menu scene, and then it's a problem because you don't have the correct object assigned to the cont button variable"); }
    }

    public void Level1()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(1, "WaveLevel");
        ClickSound();
        //levelIndex = 2;
    }

    public void Level2()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(2, "WaveLevel 2");
        ClickSound();
        //levelIndex = 3;
    }
    public void Level3()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(3, "EasyHive");
        ClickSound();
        //levelIndex = 3;
    }

    public void Level4()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(4, "WaveLevel_3");
        ClickSound();
        //levelIndex = 4;
    }
    public void Level5()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(5, "TheHiveLevel");
        ClickSound();
        //levelIndex = 3;
    }
    public void Level6()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(6, "TheSkyFalls");
        ClickSound();
        //levelIndex = 3;
    }
    public void Level7()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(7, "ArenaCombat");
        ClickSound();
        //levelIndex = 3;
    }

    public void Tutorial()
    {
        AMS_UniversalFunctions.GoToMissionBreifing(8, "Tutorial");
        ClickSound();
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("levelSelect", LoadSceneMode.Single);
        ClickSound();
    }

    public void Quit()
    {      
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        ClickSound();
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsMenu", LoadSceneMode.Single);
        ClickSound();
    }

    public void StartScreen()
    {
        //StaticCheckpointController.MovingToNewLevel();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        ClickSound();
    }

    public void Options()
    {
        ClickSound();
        OptionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Resume()
    {
        ClickSound();
        OptionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainOptionsMenu()
    {
        ClickSound();
        OptionsMenu.SetActive(true);
        mainmenu.SetActive(false);
    }

    public void MainMenuResume()
    {
        ClickSound();
        if (OptionsMenu != false) { OptionsMenu.SetActive(false); }
        if (controlsMenu != false) { controlsMenu.SetActive(false); }
        if (mainmenu != false) { mainmenu.SetActive(true); }
    }
    
    public void ContinueGame()
    {
        var Loader = FindObjectOfType<NewSaveAndLoad>();
        if(Loader != null)
        {
            var path = Loader.dataPathPos;
            if (Loader.SaveExists(path))
            {
                Vector3 pos = Loader.LoadPlayerPosition(path);
                StaticCheckpointController.SetupHere(pos);
                var smList = Loader.LoadCheckpoints();
                if (smList == null) Debug.Log(smList);
                Debug.Log(smList.Length);
                Debug.Log("HERE");
                StaticCheckpointController.checkPointList = smList;
                SceneManager.LoadScene("BigLevel1", LoadSceneMode.Single);
            }
        }
        else { Debug.Log("there should be a save and load prefab in the scene"); }
    }

    public void ClickSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }
    public void ControlsMenu()
    {
        ClickSound();
        if (mainmenu != null) { mainmenu.SetActive(false); }
        if (controlsMenu != null) { controlsMenu.SetActive(true); }

    }

}
