using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_TutorialTextUpdater : MonoBehaviour
{
    private AMS_TextController textController;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        textController = GameObject.FindObjectOfType<AMS_TextController>().GetComponent<AMS_TextController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            textController.SkipToNextText();
            Destroy(gameObject);
        }
    }
}
