using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantResourceScript : MonoBehaviour
{
    public int resourceValue;

    private bool movingUp = false;

    public float upSpeed;
    public float upTime;            //currently superceded by the shrinking
    public float shrinkSpeed;
    public AudioClip pickupSound;
    private AMS_ResourceController resourceController;
    public int scoreGainedOnCollection = 0;

    // Start is called before the first frame update
    void Start()
    {
        resourceController = FindObjectOfType<AMS_ResourceController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingUp)
        {
            var cScale = transform.localScale.x;
            cScale = Mathf.Max(cScale - shrinkSpeed, 0f);

            if (upTime <= 0f || cScale == 0f)
            {
                //    Destroy(gameObject); // Move to destroy the game object after the coroutine has ran
            }
            upTime -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + upSpeed, transform.position.z);
            transform.localScale = new Vector3(cScale, cScale, cScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!movingUp && other.name == "MainPlayer")
        {
            AMS_ScoreController.increaseScore(scoreGainedOnCollection);
            var resourceController = GameObject.Find("Resource_Controller").GetComponent<AMS_ResourceController>();
            if (resourceController != null)
            {
                resourceController.currentResources += resourceValue;
                resourceController.addedResources.text = "+ " + resourceValue;
                {
                    StartCoroutine("InstantResourceGuiTimer");
                //  Debug.Log("Coroutine has started");
                }
            }
            else
            {
                Debug.Log("Can't find resource controller");
            }
            var aSwitcher = other.GetComponent<AudioSwitcherScript>();
            if (aSwitcher != null)
            {
                aSwitcher.PlaySound(pickupSound);
            }
            else { Debug.Log("can't find audio switcher"); }

            movingUp = true;
        }
    }


    private IEnumerator InstantResourceGuiTimer ()
    {
        yield return new WaitForSeconds(3);
        resourceController.addedResources.text = " ";
     // Debug.Log("WaitForSeconds ran");
        Destroy(gameObject);
    }


}



