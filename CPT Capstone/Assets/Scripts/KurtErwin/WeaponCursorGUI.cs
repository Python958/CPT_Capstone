using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponCursorGUI : MonoBehaviour
{
    public Image backGround;
    public Image ammo;
    public Image reloadProgress;
    public Image reloadTarget;
    public Image genericCursor;
    public Image weaponCursor;
    public Image buyMenuCursor;

    private int cursorType = 2;
    private GameObject player;
    private RectTransform pos;
    private Vector2 screenCenter;

    private AMS_BuyMenuPlayer buyMenuPlayer;
    private float zAxisPos = -235f;

    private bool changedOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        player = GameObject.Find("MainPlayer");
        if (player == null) { Debug.Log("player not found!"); }

        pos = GetComponent<RectTransform>();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        buyMenuPlayer = GameObject.FindObjectOfType<AMS_BuyMenuPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetAsLastSibling();
        if (Cursor.visible) { Cursor.visible = false; }
        pos.transform.position = Input.mousePosition;
        var cursorRotation = Mathf.Atan2(Input.mousePosition.y - screenCenter.y, Input.mousePosition.x - screenCenter.x) * Mathf.Rad2Deg;
        cursorRotation -= (.75f / 2) * 360;
        pos.transform.rotation = Quaternion.Euler(0f, 0f, cursorRotation);

        backGround.gameObject.SetActive(false);
        ammo.gameObject.SetActive(false);
        reloadProgress.gameObject.SetActive(false);
        reloadTarget.gameObject.SetActive(false);
        genericCursor.gameObject.SetActive(false);
        buyMenuCursor.gameObject.SetActive(false);
        BuyMenuCursor();

        if(player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            var buyMenu = FindObjectOfType<AMS_BuyMenuPlayer>();
            if (gun != null && (buyMenu == null || buyMenu.isOpen == false))
            {
                if (cursorType == 0) { weaponCursor.gameObject.SetActive(true); }
                else if (cursorType == 1) { genericCursor.gameObject.SetActive(true); weaponCursor.gameObject.SetActive(false); }
                else
                {
                    backGround.gameObject.SetActive(true);
                    weaponCursor.gameObject.SetActive(true);
                    if (gun.reloading)
                    {
                        reloadProgress.gameObject.SetActive(true);
                        reloadTarget.gameObject.SetActive(true);

                        {
                            float rotateAmount = -1 * (gun.fastReloadMarker * (.75f * 360f));
                            float bandThickness = gun.fastReloadSpread * .75f;

                            reloadTarget.fillAmount = bandThickness;

                            var reloadTargetTrans = reloadTarget.gameObject.GetComponent<RectTransform>();
                            reloadTargetTrans.SetPositionAndRotation(reloadTargetTrans.position, pos.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, rotateAmount)));
                        }//this deals with the target band for a fast reload
                        {
                            float reloadAmount = gun.ReloadPercComplete() * .75f;
                            reloadProgress.fillAmount = reloadAmount;
                        }//this deals with the progress of the reload

                    }
                    else
                    {
                        ammo.gameObject.SetActive(true);

                        var showPercent = .75f * gun.AmmoPercent();

                        ammo.fillAmount = showPercent;
                    }
                }
            }
        }
    }

    public void SetCursorStatus(int state)
    {
        cursorType = state;
    }//states should be 0 = no cursor / 1 = generic cursor / anyElse = gun cursor

    public void BuyMenuCursor()
    {
        if(player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            if (GameObject.FindObjectOfType<AMS_BuyMenuPlayer>().isOpen == true)
            {
                gun.enabled = false;
                changedOnce = false;
                weaponCursor.gameObject.SetActive(false);
                buyMenuCursor.gameObject.SetActive(true);
                buyMenuCursor.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zAxisPos);
            }

            if (GameObject.FindObjectOfType<AMS_BuyMenuPlayer>().placing == true)
            {
                gun.gunEnabled = false;
                changedOnce = false;
                cursorType = 1;
                genericCursor.gameObject.SetActive(true);
                weaponCursor.gameObject.SetActive(false);
            }
            else if ((GameObject.FindObjectOfType<AMS_BuyMenuPlayer>().isOpen == false) || GameObject.FindObjectOfType<AMS_BuyMenuPlayer>().placing == false)
            {
                var playerScript = player.GetComponent<KE_MainPlayer_Script>();

                if (!changedOnce && !playerScript.carrying)
                {
                    gun.gunEnabled = true;
                    changedOnce = true;
                }
            }
        }
        
    }
}
