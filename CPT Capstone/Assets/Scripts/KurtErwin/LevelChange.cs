using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int nextLevelInt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StaticCheckpointController.MovingToNewLevel();
            SceneManager.LoadScene(nextLevelInt);
        }
    }
}
