using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealth : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public Animator anim;
    public bool invulnerable = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        transform.localScale = new Vector3(8f, 3, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHP <= 0)
        {
            anim.SetTrigger("PlayDeath");
        }//do something terrible

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("AfterDestroyBase"))
        {
            //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //  AMS_UniversalFunctions.GoToResultsScreen(false);
        }
    }
}
