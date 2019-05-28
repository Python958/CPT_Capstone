using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameController;

public class AMS_ResourceController : MonoBehaviour
{
    // Main Resource Value
    public int currentResources;
    //How many resources a player has at the begining of a level
    public int startingResources;
    //How many resources the player needs in order to win
    public int winConditionResources;
    private ResourceScript resourceScript;

    //GUI Text
    public Text text;
    public Text addedResources; 

    //How many resources to spawn in the world
    [Header("Must be less than or equal to number of spawners in the scene!")]
    public int totalResourcesSpawned;

    private bool hasWon = false;
    public bool playerLoad;

    // Start is called before the first frame update
    void Start()
    {
        currentResources = startingResources;
    }
    // Update is called once per frame
    void Update()
    {
        //Check to see if the player has won
        if (!hasWon && currentResources >= winConditionResources)
        {
            hasWon = true; //not sure we need this anymore but leaving incase someone does KE
        }
        //Update the GUI
        text.text = "Resources: " + currentResources;
        //Old GUI  text.text = "Resources: " + currentResources + " / " + winConditionResources;
    }
    /*  void WaitPlayerLoad()
      {
          PlayerData data = GameController.LoadPlayer();
          if (data != null)
          {
              playerLoad = data.playerLoad;
              if (playerLoad == true)
              {
                  LoadPlayer();
                  playerLoad = false;
                  data.playerLoad = false;
              }
          }
      }
      public void LoadPlayer()
      {
          PlayerData data = GameController.LoadPlayer();
          currentResources = data.resources;
      }*/
}
