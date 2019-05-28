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
        "Enemy Fortifications",
        "Your First Shack",
        "Lure out the HVT",
        "Find the Ultra Rare Resource",
        "The Final Invasion",
        "Escape without being seen",
        "Bonus Round",
        "General Gnomes Bootcamp For Dummies"
    };
    private string[] missionBreifingText = {
        "Since you have proven yourself, we are moving you near to an extremely nice shack. Problem is those pesky animals have setup fortifications in the area. If you could take them out while you are there that would be greatly appreciated.",
        "Here is your first gnome shack it’s in an extremely safe part of the backyard so you should only have to deal with a couple of invaders. If you can hold off the invaders for six days, we will upgrade you to a new and better shack.",
        "Your mission, should you choose to accept it, will take you to the Cardinal Halls. This is the home of a very powerful HVT (High Value Target). Taking him out will prove benefitial to you in the future. Be warned though, if you do not fight your way through his soldiers. They will assist him in your battle, increasing the chance of failure. \n It is only you on this mission. If you fail or start to become overrun, there will be no help. \n \n Your EVAC route is the same way you came in, do NOT let it be destroyed! Good luck Soldier.... Over and out!", // mission brief for "MidLevel (Boss)"
        "The Fortress, or the House of Doors as many know it, is a symmetrical corridor system that houses an item of Legends. At this moment, a race has begun in search of this item, what it does and why people want it is unknown, but it is up to you locate it. On start, all doors will be locked as a safety precaution and can only be unlocked by resources. According to our intel, The Fortress has a teleporter system that was used to fast travel to different locations within it, these teleporters have infinite uses. From our knowledge, only someone tagged with a special ID coded as the “Player” will be able to travel freely through them. Even if this person is carrying something they can go through, however, the item will not follow! Luckily, we have one of the last remaining IDs left. Use it wisely…",
        "URGENT MESSAGE: The largest invasion the wildlife creatures have ever done is underway. You have proven yourself to be an effective gnome fighter in the past, so you are being moved to the center of the action. The enemy is going to be dropping bombs all over the area so watch out. General gnome assures me the bombs won’t hurt your shack, but you aren’t so lucky. Be on the lookout while your there we have info that the invading forces leader Commander Cow is assisting in the attack. She will be a difficult adversary but must be defeated.",
        "You are alone out there, the enemy has found their way into your Hotel. You must get out as swiftly as possible; the longer you stay the more likely you are to be caught without any protective gear. The enemy has also brought in some makeshift gates, and they have hidden keys throughout the floor. Get to the elevator! ",
        "After seeing your skills at defending your shacks the gnomegineers have decided it might boost morale to have you fight in an arena for everyone to see. Don’t worry everything is perfectly safe, I think. So, go out there and have fun committing mass murder in front of an adoring crowd.",
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
