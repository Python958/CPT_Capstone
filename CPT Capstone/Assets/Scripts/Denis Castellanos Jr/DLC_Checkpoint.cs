using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLC_Checkpoint : MonoBehaviour
{

    public enum State {Inactive, Active};
    public State status;
    public DLC_CheckPointController checkpointController;



    // Start is called before the first frame update
    void Start()
    {
        checkpointController = FindObjectOfType<DLC_CheckPointController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (StaticCheckpointController.FindCheckPointValue(this))
            {

                StaticCheckpointController.SetCheckPointValue(this);
                Debug.Log("The Player activated checkpoint");
                checkpointController.UpdateCheckpoints(this.gameObject);
                var pos = other.gameObject.transform.position;
                StaticCheckpointController.SetupHere(pos);

                var Loader = FindObjectOfType<NewSaveAndLoad>();
                if (Loader != null)
                {
                    var path = Loader.dataPathPos;
                    Loader.DeleteSave(path);
                    Loader.SavePlayerPosition(pos, path);
                    Loader.SaveCheckpoints(StaticCheckpointController.checkPointList);
                }
                else { Debug.Log("there should be a save and load prefab in the scene if this is the big level, otherwise no save for you"); }
            }
        }
    }
}
