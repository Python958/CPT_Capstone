using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUpdates : MonoBehaviour
{
    public GameObject ObjectTryingToFind; 
    //public GameObject BaseUpdater; //The Trigger's Parent GameObject
    public GameObject TargetIndicator; // The Arrow's Parent GameObject
    public GameObject PointerObject; //The Arrow
    public GameObject BaseWarning; // The Text Objects Parent
    public Text BaseWarningText;
    public BaseHealth BaseHealthScript;

    public float StartingBaseHealth;
    public float oldHealth;
    public float currentBaseHealth;
    private float startingCountdowncheck = 3f; 
    private float countdownCheck;

    public DistanceCheck distanceCheckScript;

    // Start is called before the first frame update
    void Start()
    {
        //BaseUpdater = GameObject.Find("BaseUpdates");
        //if(BaseUpdater = null) { Debug.Log("BaseUpdater is not in the scene"); }

        TargetIndicator = GameObject.Find("TargetIndicator");
        PointerObject = TargetIndicator.transform.Find("Arrow").gameObject;
        PointerObject.SetActive(false);
        ObjectTryingToFind = GameObject.Find("Base");
        PointerObject.SetActive(false);
        BaseWarning = GameObject.Find("BaseWarning");
        BaseWarningText = GameObject.Find("BaseWarningText").GetComponent<Text>();
        BaseWarningText.GetComponent<Text>().text = "Your Base Is Under Attack!!!";
        BaseWarningText.enabled = false;
        BaseHealthScript = FindObjectOfType<BaseHealth>();
        StartingBaseHealth = BaseHealthScript.maxHP;
        oldHealth = StartingBaseHealth;
        currentBaseHealth = BaseHealthScript.currentHP;
        countdownCheck = startingCountdowncheck;
        distanceCheckScript = FindObjectOfType<DistanceCheck>();
        

    }

    private void Update()
    {
        currentBaseHealth = BaseHealthScript.currentHP;
        HasBaseBeenDamaged();
        BaseVisibleBool();
    }

    private void LateUpdate()
    {
        if(ObjectTryingToFind != null)
        {
            Vector3 ObjectTryingToFindPostion = new Vector3(ObjectTryingToFind.transform.position.x, transform.position.y, ObjectTryingToFind.transform.position.z);
            transform.LookAt(ObjectTryingToFindPostion);
        }   
    }

 /*   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PointerObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PointerObject.SetActive(true);
          
        }
    }
*/
    public bool HasBaseBeenDamaged()
    {
        if (currentBaseHealth != oldHealth && currentBaseHealth < oldHealth)
        {
            countdownCheck = countdownCheck - Time.deltaTime;
            //Debug.Log(countdownCheck);
            BaseWarningText.enabled = true;
        
            if (countdownCheck <= 0.0f)
            {
                oldHealth = currentBaseHealth;
                //Debug.Log("The timer is 0");
                if (oldHealth != currentBaseHealth)
                {
                //    Debug.Log("The Base is still being attacked");
                }
            }
            if (currentBaseHealth <= 0)
            {
                BaseWarningText.enabled = false;
            }
            //    Debug.Log("The Base is being attacked");
            return (true);
        }
        else
        {
            countdownCheck = startingCountdowncheck;
            BaseWarningText.enabled = false;
           // Debug.Log(countdownCheck);
           // Debug.Log("The Base is NOT being attacked");
            return (false);
        }
    }

     void BaseVisibleBool()
    {
        if (distanceCheckScript.DistanceBetweenBase > 30)
        {
            PointerObject.SetActive(true);
        }
        else
        {
            PointerObject.SetActive(false);
        }
    }
}
