using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
 //   public TextMeshProUGUI timer_Txt;
   // public TextMeshProUGUI levelkills_Txt;
   // public TextMeshProUGUI totalkills_Txt;
    private  EnemyHandler enemyhandler;
    private  Map mapRef;
    public GameObject[] panels;
    public GameObject[] contentobj;
    //public Image playerHelthBar;
   // public bool gamestart = false;
    public static int levelsUnlock=1;
    
   // public int unlockedLevels;
   /* public int totalkillsVal;
    public int levelkillVal=0;*/
    //public int timerVal=0;
    //public float timer = 0;

    private void Awake()
    {
        enemyhandler =  FindObjectOfType<EnemyHandler>();
        mapRef = FindObjectOfType<Map>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //totalkillsVal = PlayerPrefs.GetInt("totalkills", 0);
       // totalkills_Txt.text = totalkillsVal + " ";
      //  timer_Txt.text = timerVal + "";
       
        UiPanels_Unlock();
        Levle_Buttons_Unlock();

    }
    private void Update()
    {/*
        if (gamestart == true)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                timerVal -= 1;
                int minutes = timerVal / 60;
                int seconds = timerVal % 60;
                timer_Txt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                timer = 0f;
                if (timerVal < 1)
                {
                    timerVal = 0;
                }
            }
        }
        if (timerVal < 1)
        {
            if (enemyhandler.newObjs.Count != 0)
            {
                panels[2].gameObject.SetActive(true);
            }
        }*/
    }


    private void UiPanels_Unlock()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].gameObject.SetActive(false);
        }
        if (GameStatics.firstStart == false)
        {
            panels[0].gameObject.SetActive(true);
        }
        else
        {
            panels[1].gameObject.SetActive(true);
        }
    }

    private void Levle_Buttons_Unlock()
    {
       GameStatics.unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 1);
        for (int i = 0; i < contentobj.Length; i++)
        {
            contentobj[i].transform.gameObject.GetComponent<Button>().interactable = false;
        }
        if (GameStatics.unlockedLevels > 10)
        {
            GameStatics.unlockedLevels = 10;
        }
        // contentobj[0].transform.gameObject.GetComponent<Button>().interactable = true;
        for (int i = 0; i < GameStatics.unlockedLevels; i++)
        {
            contentobj[i].transform.gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void Start_Btn_Click()
    {     
        panels[0].gameObject.SetActive(false);
        panels[1].gameObject.SetActive(true);
        GameStatics.firstStart = true;
    }
   
    
    
    public void Level_Selection_Btn_Click(int levelVal)
    {
        GameStatics.gameStart = true;
        // Debug.Log("WEt : " + mapRef.world_Enemy_transform.Count);
        GameStatics.currentlevelVal = levelVal;
        //enemyhandler.val = levelVal;
        panels[1].gameObject.SetActive(false);
       // enemyhandler.Play_Start();
       // mapRef.EnemyMapIconsInst(mapRef.world_Enemy_transform.Count);
        Debug.Log("currentLevel : " + GameStatics.currentlevelVal);


        GameStatics.timerVal = (GameStatics.currentlevelVal == 0) ? 20 : (GameStatics.currentlevelVal  == 1) ? 35 : (GameStatics.currentlevelVal == 2) ? 60 : (GameStatics.currentlevelVal == 3) ? 80 :
                                  (GameStatics.currentlevelVal == 4) ? 100 : (GameStatics.currentlevelVal     == 5) ? 130 : (GameStatics.currentlevelVal == 6) ? 150 :
                                  (GameStatics.currentlevelVal == 7) ? 180 : (GameStatics.currentlevelVal == 8) ? 200 : 225;
        //SceneManager.LoadScene(1);
        LoadingScreenManager.loadingscreenmanager_instance.SwitchtoScene(1);


    }
}
