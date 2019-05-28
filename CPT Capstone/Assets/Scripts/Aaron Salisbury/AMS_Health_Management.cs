using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class AMS_Health_Management : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth; //DLC made public to access in follow script
    private bool ignoreDestroy = false;
    private bool despawn = false;
    public float despawnTime = 5;
    public float smTimeLeft = 3;
    public bool halfHealth = false;
    public bool isPaused = false;
    public Image healthDisplay;
    private float displayAmount;
    public bool boss;
    public bool bossDead;
    public bool isTakingDamage;
    public bool Arena;

    //Is not required for player
    public GameObject[] spawnOnDeath;
    //All numbers in array should not add up to over 1
    public float[] spawnOnDeathChances;

    //Sound Effects
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public float sceneTime = 1.5f;

    //Used for invisbility frames
    private bool invulnerable = false;
    public float invulnerableCooldown;
    private float invulnerableCurrent;
    private bool invulnerablePermanent = false;      //used for cheat codes to make player invulnerable
    public bool playerDead;
    public bool animPlayed = false;
    public MonoBehaviour[] scriptsForDeath;
    public string[] invulnerableTo;
    public GameObject FloatingDamageTextObject;
    // public AMS_GunManagement GunManagementScript;
    // public GameObject currentBullet;
    // public AMS_Bullet currentBulletScript;
    // private int currentBulletContactDamge;
    //Used for Tutorial Targets
    private bool tutorialOnce = false;

    public int scoreGainedOnDeath = 0;
    // Start is called before the first frame update
    private KE_MainPlayer_Script mainPlayerScript;
    //For weakness zone
    [HideInInspector]
    public int healthWeakend = 0;
    private int damageAmount;
    private int healthOnce = 0;

    private float baseScale;
    private float currentScale;
    private float currentScaleGrowth = 1;

    private bool deathonce = true;

    private MenuController menuController;

    //used to make sure that the hurt sound doesnt play after death
    public bool amDead = false;

    void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        playerDead = false;
        currentHealth = maxHealth;
        if (gameObject.tag == "Player")
        {
            mainPlayerScript = GetComponent<KE_MainPlayer_Script>();
        }
        isTakingDamage = false;

        baseScale = transform.localScale.y;
        currentScale = baseScale;

        amDead = false;
        // GunManagementScript = GameObject.Find("MainPlayer").GetComponent<AMS_GunManagement>();
        // currentBullet = GunManagementScript.currentBullet;
        // currentBulletScript = currentBullet.GetComponent<AMS_Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invulnerable)
        {
            currentScale += Time.deltaTime * currentScaleGrowth;
            if(Mathf.Abs(currentScale) > 1.1) { currentScaleGrowth *= -1; }
            transform.localScale = new Vector3(transform.localScale.x, currentScale, transform.localScale.z);
        }
        else if(currentScale != baseScale)
        {
            currentScale = baseScale;
            transform.localScale = new Vector3(transform.localScale.x, currentScale, transform.localScale.z);
        }

        isPaused = menuController.isPaused;
        if (despawn)
        {
            DespawnTimer();
        }
        if (gameObject.tag == "Player")
        {
            //Update the GUI
            CheckForPassiveuUpgrade();
            HudHealth();
            ReduceInvulnerable();
            if (currentHealth < 50)
            {
                SlowMotion();
                halfHealth = true;
            }
            //Resets slowmotion Time when health is above a threshhold. 
            if (currentHealth > 35)
            {
                smTimeLeft = 5;
                halfHealth = false;
            }
        }
        if (playerDead == true)
        {
            amDead = true;
            PlayerDeath();
        }
        //        currentBulletMethod();
    }

    //Used for taking damage
    public void TakeDamage(int damage, string type)
    {
        //Checks to see if the gameobject is invincible 
        if (!invulnerable && !invulnerablePermanent)
        {
            bool check = false;
            for (int i = 0; i < invulnerableTo.Length; i++)
            {
                if (type == invulnerableTo[i])
                {
                    check = true;
                }
            }
            if (!check)
            {
                currentHealth -= damage;
                damageAmount = damage;
                if (FloatingDamageTextObject != null)
                {
                    FloatingDamageText();
                    //Debug.Log("This should be working");
                }
                isTakingDamage = true;
                if (currentHealth <= 0)
                {
                    DeathSound();
                    Death(type);
                    isTakingDamage = false;
                    if (FloatingDamageTextObject != null) { FloatingDamageTextObject.SetActive(false); } //added line to stop showing damage is being taken when the player dies.
                    else { Debug.Log("couldn't find FloatingDamageTextObject!"); }

                    amDead = true; //Stop hurt sound when the player is dead.
                    
                }
                else
                {
                    if (amDead == false)
                    {
                        PlayHurtSound();
                        FloatingDamageTextObject.SetActive(true); //added line to reactivate floating damage text on respawn.
                    }
                    if (gameObject.tag == "Player")
                    {
                        invulnerable = true;
                        invulnerableCurrent = invulnerableCooldown;
                    }
                }
            }
        }
    }

    //This is used for the debug console damage or any damage that wants to ignore causing or using invulnerability
    public void TakeDamageUnfair(int damage)
    {
        currentHealth -= damage;
        isTakingDamage = false;
        PlayHurtSound();

        if (currentHealth <= 0)
        {
            Death("default");
            isTakingDamage = false;
        }
    }

    private void ModifyScoreGain(string type)
    {
        //Debug.Log("Damage Type Received: " + type);
        //Types That Decrease The Values
        if (type == "Trap" || type == "Turret")
        {
            scoreGainedOnDeath = Mathf.RoundToInt(scoreGainedOnDeath * 0.5f);
        }
        //Types That Increase The Value
        if (type == "Explosion" || type == "enviroment")
        {
            scoreGainedOnDeath = Mathf.RoundToInt(scoreGainedOnDeath * 1.5f);
        }       
    }

    //Used for trigger the death Sequence
    //KAE - a note on (string type). it doesn't seem to be used so I put "default" as the type on my TakeDamageUnfair() script.
    public void Death(string type)
    {
        ModifyScoreGain(type);
        //Increase Score
        if (scoreGainedOnDeath != 0)
        {
            AMS_ScoreController.increaseScore(scoreGainedOnDeath);
        }
        //Specialized Deaths
        if (gameObject.tag == "Player")
        {
            Debug.Log("Player DieDied");
            ignoreDestroy = true;
            playerDead = true;
            amDead = true;
        }
        if (gameObject.tag == "Default_Enemy")
        {
            if (boss == true)
            {
                bossDead = true;
            }
            DefaultEnemyDeath();

        }
        /*  if (gameObject.tag == "StaticDefence")
          {
              ignoreDestroy = false;
          }
          if(gameObject.tag == "Turret")
          {
            //  ignoreDestroy = false;
          }*/
        if (gameObject.tag == "Target")
        {
            TargetDeath();
        }
        if (gameObject.GetComponent<AMS_Hive>() != null)
        {
            gameObject.GetComponent<AMS_Hive>().LastHive();
        }
        //Default Death
        if (!ignoreDestroy)
        {
            if (gameObject.tag != "Hazard")
            {
                Destroy(gameObject);
            }
        }
    }

    //What happens when an object is marked for Despawning
    public void DespawnTimer()
    {
        //Checks to see if it is time to destroy
        if (despawnTime < 0)
        {
            //Deletes the parent gameobject
            Destroy(transform.parent.gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            //Reduces time till despawn
            despawnTime -= Time.deltaTime * 1;
        }
    }

    public void SlowMotion()
    {
        if (halfHealth == true && isPaused != true)
        {
            //Slowmotion for when the player is below 35 health.
            Time.timeScale = .8f;

            smTimeLeft -= Time.deltaTime;

            //Resets game time scale after the timer reaches 0.
            if (smTimeLeft <= 0)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void PlayerDeath()
    {
     /*   if (Arena == true)
        {

         // AMS_UniversalFunctions.GoToResultsScreen(true);
        }
        else
    */    //{
            sceneTime -= Time.deltaTime;
            if (sceneTime <= 0)
            {
              //AMS_UniversalFunctions.GoToResultsScreen(false);
            }
            if (animPlayed == false)
            {
                gameObject.GetComponent<Animation>().Play();
                animPlayed = true;
            }
            for (int i = 0; i < scriptsForDeath.Length; i++)
            {
                scriptsForDeath[i].enabled = false;
            }
       // }
    }

    public void HudHealth()
    {
        //Get the health percentage
        displayAmount = currentHealth / maxHealth;
        //Reduce it by 25 percent since it starts at .75
        displayAmount = displayAmount * 1f;
        //displayAmount = displayAmount * .75f;
        //Display it
        healthDisplay.fillAmount = displayAmount;
    }

    public void DefaultEnemyDeath()
    {
        //Make it so it wont destroy
        ignoreDestroy = true;
        //Play Death Animation
        var animation = gameObject.GetComponent<Animation>();
        if(animation != null) { animation.Play(); }


        //Turn off box collider
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        if(boxCollider != null){ boxCollider.enabled = false; }
            
        //Go through despawn system
        despawn = true;
        //Have a chance to spawn resource
        DeathSpawn();

        var navMesh = transform.parent.GetComponent<NavMeshAgent>();

        if (navMesh != null)
        {
            navMesh.isStopped = true;
        }

        else { Debug.Log("can't find navmesh on agent"); }

        var stateController = transform.parent.GetComponent<DLC_StateController>();
        if (stateController != null)
        {
            if (!stateController.enemyStats.tauntWorks)
            {
                var shield = FindObjectOfType<ForceField>();
                if (shield != null){ Destroy(shield.gameObject); }
                else { }//couldn't find a shield

                var bombScript = FindObjectOfType<RandomBomberScript>();
                if (bombScript != null) { bombScript.targetPlayer = false; }//turn off targeting player
            }//if taunt doesn't work then this is the boss
            stateController.enabled = false;
        }
        else { Debug.Log("can't find state controller of dead enemy"); }
      //BossWinCondition();
    }

    private void TargetDeath()
    {
        if (!tutorialOnce)
        {
            GameObject.FindObjectOfType<AMS_TextController>().SkipToNextText();
            tutorialOnce = true;
        }
    }

    public void DeathSpawn()
    {
        float tempRandomNumber = Random.Range(0f, 1f);
        float currentChanceCheck = 0f;
        float totalChance = 0;
        foreach (float chance in spawnOnDeathChances) { totalChance += chance; }
        float targetChance = totalChance * tempRandomNumber;
        for (int i = 0; i < spawnOnDeath.Length; i++)
        {
            currentChanceCheck += spawnOnDeathChances[i];
            if (targetChance <= currentChanceCheck)
            {
                Instantiate(spawnOnDeath[i], gameObject.transform.position, Quaternion.identity);
                //Stop the loop once one spawn is choosen
                break;
            }
        }
    }

    private void ReduceInvulnerable()
    {
        if (invulnerableCurrent > 0)
        {
            invulnerableCurrent -= 1 * Time.deltaTime;
        }
        else
        {
            invulnerable = false;
        }
    }

    public void ToggleInvulnerable()
    {
        invulnerablePermanent = !invulnerablePermanent;
    }

    private void PlayHurtSound()
    {
        if (gameObject.name == "MainPlayer")
        {
            var aSwitcher = gameObject.GetComponent<AudioSwitcherScript>();
            if (aSwitcher == null) { Debug.Log("can't find audio switcher"); }
            aSwitcher.PlaySound(hurtSound);
        }
        else
        {
            //anything else will need an audio source if it is going to play a hurt sound.
        }
    }

    private void DeathSound()
    {
        if (gameObject.name == "MainPlayer")
        {
            var aSwitcher = gameObject.GetComponent<AudioSwitcherScript>();
            if (aSwitcher == null) { Debug.Log("can't find audio switcher"); }
            if(deathonce)
            {
                aSwitcher.PlaySound(deathSound);
            }
            deathonce = false;
        }
        else
        {
            if (gameObject.tag != "Hazard")
            {
                gameObject.GetComponent<AudioSource>().clip = deathSound;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    /*  void BossWinCondition()
        {
            if (bossDead == true)
            {
              AMS_UniversalFunctions.GoToResultsScreen(true);
            }
        }
     */
    private void CheckForPassiveuUpgrade()
    {
        if (mainPlayerScript.passiveUpgradeLevels[0] == 2)
        {
            if (healthOnce == 0)
            {
                maxHealth += 60;
                currentHealth += 60;
                healthOnce++;
            }
        }
        if (mainPlayerScript.passiveUpgradeLevels[0] == 3)
        {
            if (healthOnce == 1)
            {
                maxHealth += 60;
                currentHealth += 60;
                healthOnce++;
            }
        }
        if (mainPlayerScript.passiveUpgradeLevels[0] == 4)
        {
            if (healthOnce == 2)
            {
                maxHealth += 60;
                currentHealth += 60;
                healthOnce++;
            }
        }
    }

    void FloatingDamageText()
    {
        if (gameObject.tag == "Default_Enemy" || gameObject.tag == "Enemy" || gameObject.tag == "Player")
        {
           var FloatingText = Instantiate(FloatingDamageTextObject, transform.position, Quaternion.identity, transform);
            FloatingText.GetComponent<TextMesh>().text = damageAmount.ToString();
        }
    }
}
