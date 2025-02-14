using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    InGameHandler ingamehandler;
   // UIHandler uihandler;
    EnemyHandler enemyHandler;
    EnemyData enemydata;
    public int playerhelthVal = 15;
    private int currenthelth;
    public int reducePlayerHelthVal;
    private float durationhelthBarVal = 0.5f;
   
    // Start is called before the first frame update
    void Start()
    {
        ingamehandler = FindObjectOfType<InGameHandler>();
        //uihandler = FindObjectOfType<UIHandler>();
        enemyHandler = FindObjectOfType<EnemyHandler>();
        enemydata = FindObjectOfType<EnemyData>();
        currenthelth = playerhelthVal;
        /*ingamehandler.playerHelthBar.fillAmount = currenthelth;
   
        ingamehandler.playerHelthBar.color = (ingamehandler.playerHelthBar.fillAmount > 0.6f) ? Color.green :
                                         (ingamehandler.playerHelthBar.fillAmount > 0.3f) ? Color.yellow : Color.red;*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            reducePlayerHelthVal =
                (enemyHandler.enemyDictionary[other.gameObject.transform.root.gameObject].zombieType == EnemyData.ZombieType.Hulk) ? 3 :
                (enemyHandler.enemyDictionary[other.gameObject.transform.root.gameObject].zombieType == EnemyData.ZombieType.Police) ? 2 : 1;

            StartCoroutine(PlayerHelthReduce(reducePlayerHelthVal));
        }
    }

    private IEnumerator PlayerHelthReduce(int reduceHelthVal)
    {
        float startHealth = (float)currenthelth / playerhelthVal;
        currenthelth -= reduceHelthVal;
        currenthelth = Mathf.Clamp(currenthelth, 0, playerhelthVal);
        float endHealth = (float)currenthelth / playerhelthVal;

        float elapedtime = 0;

        while (elapedtime < durationhelthBarVal)
        {
            elapedtime += Time.deltaTime;
            float timeVal = elapedtime / durationhelthBarVal;
            ingamehandler.playerHelthBar.fillAmount = Mathf.Lerp(startHealth, endHealth, timeVal);

            ingamehandler.playerHelthBar.color = (ingamehandler.playerHelthBar.fillAmount > 0.6f) ? Color.green :
                                             (ingamehandler.playerHelthBar.fillAmount > 0.3f) ? Color.yellow : Color.red;
            ingamehandler.scratch_img.gameObject.SetActive(true);
            yield return null;
            ingamehandler.scratch_img.gameObject.SetActive(false);
        }

        ingamehandler.playerHelthBar.fillAmount = endHealth; // Ensure the fill amount is set to the final value

        if (currenthelth < 1)
        {
            ingamehandler.inGamepanels[0].gameObject.SetActive(true);
            GameStatics.gameStart = false;
            ingamehandler.playerHelthbarObj.gameObject.SetActive(false);
            ingamehandler.timer_Txt.gameObject.SetActive(false);
            ingamehandler.scratch_img.gameObject.SetActive(false);

        }
        yield return null;
    }

}
