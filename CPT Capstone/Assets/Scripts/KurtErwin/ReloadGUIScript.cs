using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReloadGUIScript : MonoBehaviour
{
    public RectTransform marker;
    public RectTransform goal;
    public Canvas canvas;
    private RectTransform selfT;
    private Image graphic;

    public float barWidth; //these are percentages of the main bar
    public float barHeight;
    private AMS_GunManagement gunScript;
    private float fullWidth;
    private float fullHeight;
    private bool isOn = true; //bool checks whether it has been toggled
    private bool blinkingOn = true;
    public float blinkSpeed = .15f;
    private float blinkCounter;
           
    // Start is called before the first frame update
    void Start()
    {
        var tempPlayerVar = GameObject.FindObjectOfType<KE_MainPlayer_Script>();
        if(tempPlayerVar != null)
        {
            gunScript = tempPlayerVar.gameObject.GetComponent<AMS_GunManagement>();
            if (gunScript == null) { Debug.Log("can't find gun on player"); }
        }
        else
        {
            Debug.Log("can't find player script");
        }

        graphic = GetComponent<Image>();
        if(graphic == null) { Debug.Log("no graphic found on this object"); }
        selfT = GetComponent<RectTransform>();
        if(selfT == null) { Debug.Log("no rect transform found on this object"); }

        fullWidth = (selfT.rect.width * canvas.scaleFactor);
        fullHeight = (selfT.rect.height * canvas.scaleFactor);

        if (marker != null)
        {
            var newHeight = fullHeight / 2 * barHeight;
            var newWidth = fullWidth / 2 * barWidth;

            marker.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            marker.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            marker.SetPositionAndRotation(new Vector3(selfT.position.x, selfT.position.y, selfT.position.z), goal.rotation);
        }//this sets up the scrolling marker
        else { Debug.Log("No marker found"); }

        ToggleView(false, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gunScript != null)
        {
            if (gunScript.gunEnabled && gunScript.reloading)
            {
                if (!isOn) { ToggleView(true, false); }
                if (!gunScript.fastReloadAttempted)
                {
                    var percPos = gunScript.ReloadPercComplete();
                    var newPos = fullWidth * percPos;
                    marker.SetPositionAndRotation(new Vector3(selfT.position.x + newPos, selfT.position.y, selfT.position.z), goal.rotation);
                }//haven't fast reloaded yet
                else
                {
                    if (!gunScript.fastReloadSuccess)
                    {
                        blinkCounter -= Time.deltaTime;
                        if(blinkCounter <= 0f)
                        {
                            blinkCounter = blinkSpeed;
                            blinkingOn = !blinkingOn;
                            ToggleView(blinkingOn, true);
                        }
                    }
                }//tried to fast reload

            }
            else
            {
                if (isOn) { ToggleView(false, false); }
            }
        }
        else { if (isOn) { ToggleView(false, false); } }
    }

    public void ReloadGuiSetup()
    {

        if (goal != null)
        {
            var newHeight = fullHeight/2;
            var newWidth = fullWidth/2 * gunScript.fastReloadSpread;
            var newPos = fullWidth * gunScript.fastReloadMarker;

            goal.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            goal.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            if(selfT == null)
            {
                selfT = GetComponent<RectTransform>();
            }
            goal.SetPositionAndRotation(new Vector3(selfT.position.x+newPos, selfT.position.y, selfT.position.z), goal.rotation);
            marker.SetPositionAndRotation(new Vector3(selfT.position.x, selfT.position.y, selfT.position.z), goal.rotation);
        }//This sets up the goal area

        else { Debug.Log("No goal found"); }
    }//sets up GUI in correct places
    
    private void ToggleView(bool on, bool blinking)
    {
        graphic.enabled = on;
        var markerGraphic = marker.gameObject.GetComponent<Image>();
        var goalGraphic = goal.gameObject.GetComponent<Image>();
        if (markerGraphic == null || goalGraphic == null) { Debug.Log("marker and/or goal didn't have a graphic"); }
        else
        {
            markerGraphic.enabled = on;
            goalGraphic.enabled = on;
        }

        if (!blinking)
        {
            ReloadGuiSetup();
            isOn = on;
        }
    }//changing the visiblility of the elements
}
