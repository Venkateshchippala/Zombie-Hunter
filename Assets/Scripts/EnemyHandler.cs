using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.XR;

public class EnemyHandler : MonoBehaviour
{
    
    private InGameHandler ingamehandler;
    // public PlayerHandler playerHandler;
    public Transform plyer;
    public List<GameObject> enemys = new List<GameObject>();
    // Dictionary to store enemy GameObject and corresponding EnemeyData
    public Dictionary<GameObject, EnemyData> enemyDictionary = new Dictionary<GameObject, EnemyData>();
    public List<GameObject> newObjs= new List<GameObject>();
    // public List<GameObject> levels = new List<GameObject>();
    public Transform[] levels;
    public Map mapRef;
    public GameObject scratchImg;

    public int normal_zomby_count = 5;
    public int medium_zomby_count = 4;
    public int heard_zomby_count = 3;

    int enemycount;
    int instantiateObjchilds;
    public int val;
   // public int currentlevel = 0;
    public float enemyHelth;
    GameObject _enemy;
    int positionval=0;
    int instantiateval = 1;
    int liveEnemys;
    public bool near = false;
    // [SerializeField] Map _map;



    // EnemeyData e_one_data;
    private void Awake()
    {
        val = GameStatics.currentlevelVal;
       
        mapRef = FindObjectOfType<Map>();
        ingamehandler = FindObjectOfType<InGameHandler>();
       
    }
    private void Start()
    {
        if (GameStatics.gameStart == true)
        {
            Play_Start();
            mapRef.EnemyMapIconsInst(mapRef.world_Enemy_transform.Count);
        }
        // currentHealth = maxHealth; // Initialize with maximum health
        // Play_Start();

    }
    public void Play_Start()
    {
        
        
            //currentlevel = val;
            // Debug.Log("Static val : " + val);
            instantiateObjchilds = levels[val].transform.childCount;
           // Debug.Log("child count : " + instantiateObjchilds);
            for (int i = 0; i < instantiateObjchilds; i++)
            {
                enemycount = Random.Range(0, enemys.Count);
                enemyHelth = (enemycount == 0) ? 5 : (enemycount == 1) ? 3 : 1;
                EnemyData.ZombieType type = (enemycount == 0) ? EnemyData.ZombieType.Hulk : (enemycount == 1) ?
                                                            EnemyData.ZombieType.Police : EnemyData.ZombieType.Small;
                ZomboyInstanciation(enemys[enemycount], instantiateval, levels, type);
                positionval++;
            }
            positionval = 0;
       // Debug.Log("newObjs.count : " + newObjs.Count);
        liveEnemys = newObjs.Count;
    }
   
    private void ZomboyInstanciation(GameObject Zombie, int count, Transform[] _trns, EnemyData.ZombieType zombieType)
    {
        for (int i = 0; i < count; i++)
        {
            _enemy = Instantiate(Zombie, _trns[val].transform.GetChild(positionval).transform.position, Quaternion.identity);
            _enemy.name = zombieType + "Zombie_" + i;
            //_enemy.GetComponent<NavMeshAgent>().SetDestination(plyer.position);
            EnemyData e_one_data = new EnemyData(_enemy.name,
                enemyHelth,
                plyer,
                _enemy.GetComponent<NavMeshAgent>(),
                _enemy.transform.GetChild(0).GetComponent<Animator>(), zombieType);

            // Add the enemy GameObject and its data to the dictionary
            enemyDictionary.Add(_enemy, e_one_data);
            newObjs.Add(_enemy);
            mapRef.world_Enemy_transform.Add(_enemy.transform);
            // _map.world_Enemy_transform.Add(_enemy.transform);
        }

    }

    private void Update()
    {
       /* if (uihandler.gamestart == true)
        {
            Play_Start();

        }*/
        // Iterate through the dictionary and update each enemy's destination
        if (GameStatics.gameStart == true)
        {

            EnemyMovementAndAttack();
            // enemyDictionary[_enemy].transform.LookAt(plyer.transform.position);
            if (newObjs.Count == 0)
            {
                if (val == GameStatics.unlockedLevels-1)
                {
                    //  Debug.Log(" next uihandler.currentlevel : " + uihandler.currentLevel);
                    Debug.Log(" Befor uihandler.unlockedLevels : " + GameStatics.unlockedLevels);

                    // UIHandler.levelsUnlock ++;
                    /* PlayerPrefs.SetInt("unlockedLevels", PlayerPrefs.GetInt("unlockedLevels") + 1);
                     uihandler.unlockedLevels = PlayerPrefs.GetInt("unlockedLevels");*/
                    int val = GameStatics.unlockedLevels + 1;
                    PlayerPrefs.SetInt("unlockedLevels", val);
                    PlayerPrefs.Save();
                    GameStatics.unlockedLevels = PlayerPrefs.GetInt("unlockedLevels");
                    //  Debug.Log(" next uihandler.unlockedLevels : " + uihandler.unlockedLevels);
                    Debug.Log("next uihandler.unlockedLevels : " + PlayerPrefs.GetInt("unlockedLevels"));
                }
                
                 GameStatics.gameStart = false;
                StartCoroutine(DelayLevelCompletePanel());
                ingamehandler.playerHelthbarObj.gameObject.SetActive(false);
                ingamehandler.timer_Txt.gameObject.SetActive(false);
                //uihandler.panels[3].gameObject.SetActive(true);
                //SceneManager.LoadScene(0);
            }

        }
        IEnumerator DelayLevelCompletePanel()
        {
            yield return new WaitForSeconds(2);
            ingamehandler.inGamepanels[1].gameObject.SetActive(true);
        }
        //Debug.Log("newObjs.count : " + newObjs.Count);

        if(newObjs.Count < liveEnemys)
        {
            PlayerPrefs.SetInt("totalkills", PlayerPrefs.GetInt("totalkills") + 1);
            ingamehandler.totalkillsVal = PlayerPrefs.GetInt("totalkills");
            ingamehandler.levelkillVal++;
            ingamehandler.totalkills_Txt.text = ": " + ingamehandler.totalkillsVal.ToString();
            ingamehandler.levelkills_Txt.text = ": " + ingamehandler.levelkillVal.ToString();
            liveEnemys = newObjs.Count;
        }

    }

    private void EnemyMovementAndAttack()
    {
        foreach (var pair in enemyDictionary)
        {
            EnemyData data = pair.Value;

            // Skip if enemy is dead
            if (data.isDeath)
                continue;

            if (data.navMeshAgent != null && data.targetPosition != null)
            {
                Vector3 targetPosition = data.targetPosition.position;
                float distanceToTarget = Vector3.Distance(targetPosition, data.navMeshAgent.transform.position);

                // Debug.Log("distanceToTarget : " + distanceToTarget);
                if (distanceToTarget > 3f)
                {
                    near = false;
                    // Enemy should move towards the target
                    if (data.navMeshAgent.destination != targetPosition)
                    {
                        data.navMeshAgent.SetDestination(targetPosition);
                    }

                    // Ensure attack animation is off if enemy is moving
                    if (data.anim.GetBool("attack"))
                    {
                        data.anim.SetBool("attack", false);
                        // data.StopDamage();
                        // data.isApplyDamageToPlayer = false;
                    }

                    // Set attack state back to ready
                    data.isAttack = true;
                }
                else
                {
                    // Enemy is within attack range
                    if (data.isAttack && !data.anim.GetBool("attack"))
                    {
                        near = true;
                        data.anim.SetBool("attack", true);

                        // Start a coroutine to repeatedly deal damage
                        // playerHandler.Zobmies(data);
                        // StartCoroutine(DealDamageRepeatedly(data.PlayerDamage()));
                        // Prevent multiple attack invocations
                        // data.isApplyDamageToPlayer = true;
                        //data.PlayerDamage();
                        data.isAttack = false;
                    }

                    /*  if (data.isApplyDamageToPlayer)
                      {
                          data.PlayerDamage();
                         // playerHandler.PlayerDamage(data);

                      }*/
                }

            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Missing references for enemy: {pair.Key}");
#endif
            }
        }
    }
    // it will effect on when plyer shoots the zombie. Zombie health demage and it will die
    public void EnemyDeth(GameObject zombie)
    {

        // print(zombie.name);
        EnemyHealthDamege(zombie);
        if (enemyDictionary[zombie].health <= 0)
        {
            Destroy(zombie, 2);
            newObjs.Remove(zombie);
            enemyDictionary[zombie].isDeath = true;
            enemyDictionary[zombie].navMeshAgent.isStopped = true;
            enemyDictionary[zombie].anim.SetTrigger("death");
            mapRef.RemoveIcon(zombie.transform);

        }

        // StartCoroutine(BulletHit(newObj));
    }

    public void EnemyHealthDamege(GameObject zombie)
    {
        if (enemyDictionary[zombie].health <= 0)
            return;
        enemyDictionary[zombie].health -= (enemyDictionary[zombie].zombieType == EnemyData.ZombieType.Hulk) ? 1 :
                                                   (enemyDictionary[zombie].zombieType == EnemyData.ZombieType.Police) ? 1 : 1;
      
    }
    public Vector2 healthBarSize = new Vector2(50, 5);
    public Vector3 healthBarOffset = new Vector3(0, 2, 0); // Off
    private void OnGUI()
    {

        if (GameStatics.gameStart == true)
        {
            for (int i = 0; i < newObjs.Count; i++)
            {
                if (i >= newObjs.Count)
                {
                    Debug.LogWarning("Index out of range: newObjs list is shorter than enemyDictionary.");
                    continue;
                }

                if (newObjs[i] == null)
                {
                    Debug.LogWarning("Null reference: newObjs element is null.");
                    continue;
                }

                if (!enemyDictionary.ContainsKey(newObjs[i]))
                {
                    Debug.LogWarning("Key not found: newObjs element is not in enemyDictionary.");
                    continue;
                }

                // Calculate health percentage
                float healthPercentage = (float)enemyDictionary[newObjs[i]].health / enemyDictionary[newObjs[i]].maxHealth;

                // Get the enemy's position in world space and convert it to screen space
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(newObjs[i].transform.position + healthBarOffset);

                // Only draw the health bar if the enemy is in front of the camera
                if (screenPosition.z > 0)
                {
                    // Flip the Y-coordinate because OnGUI uses a top-left origin
                    screenPosition.y = Screen.height - screenPosition.y;

                    // Calculate the health bar's rectangle
                    Rect healthBarRect = new Rect(
                        screenPosition.x - healthBarSize.x / 2,  // Center the bar horizontally
                        screenPosition.y - healthBarSize.y / 2,  // Position it slightly above the enemy
                        healthBarSize.x * healthPercentage,      // Scale width by health percentage
                        healthBarSize.y
                    );

                    // Draw the health bar background (gray)
                    GUI.color = Color.gray;
                    GUI.DrawTexture(new Rect(
                        screenPosition.x - healthBarSize.x / 2,
                        screenPosition.y - healthBarSize.y / 2,
                        healthBarSize.x,
                        healthBarSize.y
                    ), Texture2D.whiteTexture);

                    // Draw the current health bar (dynamic color)
                    GUI.color = healthPercentage > 0.7f ? Color.green : (healthPercentage > 0.4f ? Color.yellow : Color.red);
                    GUI.DrawTexture(healthBarRect, Texture2D.whiteTexture);

                    // Optional: Display health as text on top of the bar
                    GUI.color = Color.white;
                    GUI.Label(new Rect(
                        screenPosition.x - healthBarSize.x / 2,
                        screenPosition.y - healthBarSize.y - 15,  // Position the text above the health bar
                        healthBarSize.x,
                        20
                    ), $"{enemyDictionary[newObjs[i]].health}/{enemyDictionary[newObjs[i]].maxHealth}");
                }
            }
        }
           
       
        
    }

    public void ZombieHit_by_Bullet(GameObject gameobj)
    {
        StartCoroutine(ZombieAttackTakenAnim(gameobj.transform.gameObject));
    }
    private IEnumerator ZombieAttackTakenAnim(GameObject gameobj)
    {
       
        yield return new WaitForSeconds(0.5f);
      
        if(gameobj != null)
        {
            enemyDictionary[gameobj].anim.SetBool("damagetaken", false);
            if (near == true)
            {
                enemyDictionary[gameobj].anim.SetBool("attack", true);
            }
        }
       
    }



    //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$


    private void OnApplicationPause(bool pause)
    {

        if (pause)
        {
            StopDamage();
        }
        /* else {
             Debug.Log("Pase to play");
             PauseToPlay();
         }*/

    }
    private void OnApplicationQuit()
    {
        StopDamage();
    }

    private void OnApplicationFocus(bool focus)
    {
        StopDamage();
    }
    private void OnIterateMethod()
    {
        StopDamage();
    }

    void StopDamage()
    {
        foreach (var pair in enemyDictionary)
        {
            EnemyData data = pair.Value;
            //data.StopDamage();
        }
    }
    void PauseToPlay()
    {
        foreach (var pair in enemyDictionary)
        {
            EnemyData data = pair.Value;
            if (data.anim.GetBool("Attack"))
            {
                // data.PlayerDamage();
            }
        }
    }
   

}
