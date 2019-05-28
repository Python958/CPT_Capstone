using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageText : MonoBehaviour
{
    public AMS_GunManagement ams_GunManagement;
  //public AMS_Bullet ams_Bullet1Script;
  //public AMS_Bullet ams_Bullet2Script;
  //public GameObject Gun1Bullet;
  //public GameObject Gun2Bullet;
    public GameObject currentBullet;
    public AMS_Bullet currentBulletScript;
    public int currentBulletContactDamage;
  //public int Gun1BulletContactDamage;
  //public int Gun2BulletContactDamage;
 // public int TempBulletContactDamage;
//  public GameObject TempbulletObject;


    // Start is called before the first frame update
    void Start()
    {
        ams_GunManagement = GameObject.Find("MainPlayer").GetComponent<AMS_GunManagement>();
     // Gun1Bullet = ams_GunManagement.gun1Bullet;
     // Gun2Bullet = ams_GunManagement.gun2Bullet;
        currentBullet = ams_GunManagement.currentBullet;
        currentBulletScript = currentBullet.GetComponent<AMS_Bullet>();
        currentBulletContactDamage = currentBulletScript.contactDamage;
     // ams_Bullet1Script = Gun1Bullet.GetComponent<AMS_Bullet>();
     // Gun1BulletContactDamage = ams_Bullet1Script.contactDamage;




    }

    // Update is called once per frame
    void Update()
    {
        currentBulletMethod();
       // Gun2();
       // Debug.Log("The " + Gun1Bullet + " does " + Gun1BulletContactDamge);
       // Debug.Log("The active weapon uses the " + Gun1Bullet + " bullet");
    }

    void currentBulletMethod()
    {
        currentBullet = ams_GunManagement.currentBullet;
        currentBulletScript = currentBullet.GetComponent<AMS_Bullet>();

        if (currentBulletScript == null)
        {
            currentBulletScript = currentBullet.GetComponentInChildren<AMS_Bullet>();
        }
        currentBulletContactDamage = currentBulletScript.contactDamage;
    }

/*private void Gun2()
    {
        Gun2Bullet = ams_GunManagement.gun2Bullet;
        

        if (Gun2Bullet != null)
        {
            ams_Bullet2Script = Gun2Bullet.GetComponentInChildren<AMS_Bullet>();

            if (Gun2Bullet == true)
            {
                Gun2BulletContactDamage = ams_Bullet2Script.contactDamage;
            }
        }
     // Debug.Log("The " + Gun2Bullet + " does " + Gun2BulletContactDamage);
    //  Debug.Log("The active weapon uses the " + Gun2Bullet + " bullet");
    }
*/

}
