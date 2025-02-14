using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager loadingscreenmanager_instance;
    public GameObject loadingScreenObject;
    public Slider loadingBar;
   /* public GameObject rotate_Spinner;
    private float rotate_Spinner_Speed = 360f;*/
    private void Awake()
    {
        if(loadingscreenmanager_instance != null && loadingscreenmanager_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            loadingscreenmanager_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void SwitchtoScene(int id)
    {
        loadingScreenObject.gameObject.SetActive(true);
        loadingBar.value = 0;
        StartCoroutine("SwitchtoSceneAsyc",id);
    }

    IEnumerator SwitchtoSceneAsyc(int id)
    {
        AsyncOperation asycLoad = SceneManager.LoadSceneAsync(id);

        while(!asycLoad.isDone)
        {
            loadingBar.value = asycLoad.progress;
            yield return null;
        }
        yield return new WaitForSeconds(0.02f);
        loadingScreenObject.gameObject.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate_Spinner.transform.eulerAngles = new Vector3(0, 0, Time.deltaTime * rotate_Spinner_Speed);
    }
}
