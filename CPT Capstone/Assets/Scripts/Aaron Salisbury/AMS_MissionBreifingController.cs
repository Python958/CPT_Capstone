using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AMS_MissionBreifingController : MonoBehaviour
{
    //Access the UI
    [Tooltip("Order, Level Name, Mission Breifing, Objectives")]
    public Text[] displayTexts;
    public Image[] displayImages;
    public GameObject declineMissionButton;
    public GameObject skipButton;
    //Displayed based on Level
    private string[] levelNames = {
        "Your First Shack", //Level 1
        "Needs Name", //Level 2
        "Need Name", //Level 3
        "Need Name", //Level 4
        "Enemy Fortifications", //Level 5
        "The Final Invasion", //Level 6
        "Bonus Round", //Arena
        "General Gnomes Bootcamp For Dummies" // Tutorial
    };
    private string[] missionBreifingText = {
        //Level 1
        "Here is your first gnome shack it’s in an extremely safe part of the backyard so you should only have to deal with a couple of invaders. If you can hold off the invaders for six days, we will upgrade you to a new and better shack.",
        //Level 2
        "Needs a description",
        //Level 3
        "Needs a description",
        //Level 4
        "Needs a description",
        //Level 5
        "Since you have proven yourself, we are moving you near to an extremely nice shack. Problem is those pesky animals have setup fortifications in the area. If you could take them out while you are there that would be greatly appreciated.",
        //Level 6
        "URGENT MESSAGE: The largest invasion the wildlife creatures have ever done is underway. You have proven yourself to be an effective gnome fighter in the past, so you are being moved to the center of the action. The enemy is going to be dropping bombs all over the area so watch out. General gnome assures me the bombs won’t hurt your shack, but you aren’t so lucky. Be on the lookout while your there we have info that the invading forces leader Commander Cow is assisting in the attack. She will be a difficult adversary but must be defeated.",
        //Arena
        "After seeing your skills at defending your shacks the gnomegineers have decided it might boost morale to have you fight in an arena for everyone to see. Don’t worry everything is perfectly safe, I think. So, go out there and have fun committing mass murder in front of an adoring crowd.",
        //Tutorial
        "Welcome to the backyard. Before you move into your first shack general gnome would like to meet with you to go over the basics on how to defend it from those pesky forest creatures. Some gnomes choose not to visit general gnome, but some gnomes are also dead …. Sooo it’s up to you whether you want to pay him a visit. Either way welcome to the backyard and watch out for those pesky squirrels and ducks.",
    }; //Missing Briefing Text // The Fortress Level }; 

    // \n creates a new line.
    public Sprite[] leftDisplayImage;
    public Sprite[] rightDisplayImage;

    //Variables needed for functions
    private int levelNumber;
    // Start is called before the first frame update
    void Start()
    {
        
        levelNumber = AMS_UniversalFunctions.levelNumber - 1;
        InputLevelData();
        //Tutorial Skip
        if (levelNumber == 7)
        {
            skipButton.SetActive(true);
            declineMissionButton.SetActive(false);
        }
    }

    private void InputLevelData()
    {
        displayTexts[0].text = levelNames[levelNumber];
        displayTexts[1].text = missionBreifingText[levelNumber];
        displayTexts[2].text = AMS_UniversalFunctions.objectivesText[levelNumber];
        displayImages[0].sprite = leftDisplayImage[levelNumber];
        displayImages[1].sprite = rightDisplayImage[levelNumber];
    }

    public void AcceptMission()
    {
        SceneManager.LoadScene(AMS_UniversalFunctions.missionLevel);
        ClickSound();
    }
    public void RefuseMission()
    {
        SceneManager.LoadScene("levelSelect");
        ClickSound();
    }
    public void SkipMission()
    {
        if (!PlayerPrefs.HasKey("Score Tutorial"))
        {
            PlayerPrefs.SetString("Score Tutorial", "C");
            skipButton.SetActive(false);
            declineMissionButton.SetActive(true);
        }
        RefuseMission();
    }
    private void ClickSound()
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
