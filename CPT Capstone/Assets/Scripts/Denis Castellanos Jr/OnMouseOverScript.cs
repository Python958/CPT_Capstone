using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnMouseOverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LevelSelectController LevelSelectControllerScript;
    public GameObject BonusLevelButton;
    public Text BonusNotificationText;

    // Start is called before the first frame update
    void Start()
    {
        LevelSelectControllerScript = FindObjectOfType<LevelSelectController>();
        BonusNotificationText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsTheArenaUnlocked();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        BonusNotificationText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BonusNotificationText.enabled = false;
    }

    public void IsTheArenaUnlocked()
    {
        if(LevelSelectControllerScript.unlockArena == true)
        {
            BonusNotificationText.enabled = false;
        }
    }
}
