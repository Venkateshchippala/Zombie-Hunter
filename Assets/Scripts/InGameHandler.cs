using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameHandler : MonoBehaviour
{
   // private UIHandler uihandler;
    private EnemyHandler enemyhandler;
    private Map mapRef;
    public GameObject[] inGamepanels;
    public TextMeshProUGUI timer_Txt;
    public TextMeshProUGUI levelkills_Txt;
    public TextMeshProUGUI totalkills_Txt;
    public GameObject scratch_img;
    public GameObject playerHelthbarObj;
    public Image playerHelthBar;
    public int totalkillsVal;
    public int levelkillVal = 0;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        enemyhandler = FindObjectOfType<EnemyHandler>();
        mapRef = FindObjectOfType<Map>();
       // uihandler = FindObjectOfType<UIHandler>();
        totalkillsVal = PlayerPrefs.GetInt("totalkills", 0);
        timer_Txt.text = GameStatics.timerVal + " ";
        scratch_img.gameObject.SetActive(false);
        for(int i = 0; i < inGamepanels.Length; i++)
        {
            inGamepanels[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStatics.gameStart == true)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                GameStatics.timerVal -= 1;
                int minutes = GameStatics.timerVal / 60;
                int seconds = GameStatics.timerVal % 60;
                timer_Txt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                timer = 0f;
                if (GameStatics.timerVal < 1)
                {
                    GameStatics.timerVal = 0;
                }
            }
            if (GameStatics.timerVal < 1)
            {
                if (enemyhandler.newObjs.Count != 0)
                {
                    GameStatics.gameStart = false;
                    //panels[2].gameObject.SetActive(true);
                    inGamepanels[0].gameObject.SetActive(true);
                    playerHelthbarObj.gameObject.SetActive(false);
                    timer_Txt.gameObject.SetActive(false);
                }
            }
        }
    }
    public void Retry_Btn_Click()
    {
        inGamepanels[0].gameObject.SetActive(false);
        SceneManager.LoadScene(0);
       /* uihandler.panels[0].gameObject.SetActive(false);
        uihandler.panels[1].gameObject.SetActive(true);*/
        
    }

    public void Next_Btn_Click()
    {
        inGamepanels[1].gameObject.SetActive(false);
        SceneManager.LoadScene(0);
        /*uihandler.panels[0].gameObject.SetActive(false);
        uihandler.panels[1].gameObject.SetActive(true);*/

    }
}
