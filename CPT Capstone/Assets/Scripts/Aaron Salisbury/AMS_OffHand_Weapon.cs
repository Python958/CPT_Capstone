using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_OffHand_Weapon : MonoBehaviour
{
    public GameObject mine;
    public int mineThrowDistance = 5;
    public int mineAmmoMax;
    public int mineAmmoCurrent;
    public string selectedOffHandWeapon;
    private Vector3 mousePos;
    public Text OffHandAmmoText;

    // Start is called before the first frame update
    void Start()
    {
        mineAmmoCurrent = mineAmmoMax;
        OffHandAmmoText = GameObject.Find("OffHandAmmoText").GetComponent<Text>(); //Grabs the Off Hand Weapon Text
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (selectedOffHandWeapon == "Mine")
            {
                if (mineAmmoCurrent > 0)
                {
                    ThrowMine();
                    mineAmmoCurrent--;
                }
            }
        }
        OffHandAmmoText.text = "" + mineAmmoCurrent.ToString("00");
    }

    private void GetMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;
        }
    }

    private void ThrowMine()
    {
        //Where is the mouse
        GetMouse();
        GameObject tempmine = Instantiate(mine, new Vector3 (transform.position.x, transform.position.y - .9f, transform.position.z), Quaternion.identity);
        tempmine.GetComponent<AMS_ThrownMine>().dist = mineThrowDistance;
        //Make it go to where the mouse is pointing but at 0 in the y
        tempmine.GetComponent<AMS_ThrownMine>().destination = new Vector3(mousePos.x,tempmine.transform.position.y,mousePos.z);
        tempmine.GetComponent<AMS_ThrownMine>().maxDist = mineThrowDistance;
        
    }
}
