using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenuScript : MonoBehaviour
{
    public GameObject contentparent;
    public GameObject scrollbarRef;
    float scroll_pos = 0;
    float[] pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[ contentparent.transform.childCount];
        float distance = 1f / (pos.Length - 1);
        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbarRef.GetComponent<Scrollbar>().value;
        }
        else
        {
            for(int i = 0; i < pos.Length; i++)
            {
                if(scroll_pos < pos[i]+(distance/2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbarRef.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbarRef.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for(int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i]+(distance/2) && scroll_pos > pos[i] - (distance / 2))
            {
                contentparent.transform.GetChild(i).localScale = Vector2.Lerp(contentparent.transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);

                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                       contentparent.transform.GetChild(j).localScale = Vector2.Lerp(contentparent.transform.GetChild(j).localScale, new Vector2(0.7f, 0.7f), 0.1f);
                    }
                }
            }
        }
    }
}
