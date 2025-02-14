using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Boundries bounds;
    [SerializeField] private Transform worldT_Player_transform;
    [SerializeField] private RectTransform imageTransform_Plaeyr;


    private UIHandler uihandler;
    [SerializeField] private RectTransform enmay_Ui_Icon;

    public List<Transform> world_Enemy_transform = new List<Transform>();
    [SerializeField] private List<RectTransform> imageTransform_Enemy = new List<RectTransform>();

    //[SerializeField] EnemyHandler enemyHandler;

    private RectTransform MapTransform => transform as RectTransform;


    private void Awake()
    {
        // MapTransform = transform as RectTransform;
        //  EnemyMapIconsInst(4);



       // EnemyMapIconsInst(world_Enemy_transform.Count);
        uihandler = FindObjectOfType<UIHandler>();
    }

    public void EnemyMapIconsInst(int count)
    {
       // Debug.Log("count : " + count);
        for (int i = 0; i < count; i++)
        {
            RectTransform e_Icon = Instantiate(enmay_Ui_Icon, transform);
            imageTransform_Enemy.Add(e_Icon);
        }
    }

    void EnemyAnchorPositonUpdate(List<Transform> world_Enemy_transform)
    {
        for (int i = 0; i < world_Enemy_transform.Count; i++)
        {
            // Debug.Log("world_Enemy_transform.Count : " + world_Enemy_transform.Count);

            if (world_Enemy_transform[i] != null)
            {
                imageTransform_Enemy[i].anchoredPosition = -FindInterfacePoint(world_Enemy_transform[i]);
            }
        }
    }

    public void RemoveIcon(Transform _worldObj)
    {
        int index = world_Enemy_transform.IndexOf(_worldObj);

        // world_Enemy_transform.RemoveAt(index);
        // imageTransform_Enemy.RemoveAt(index);
        imageTransform_Enemy[index].gameObject.SetActive(false);
    }
    private void Update()
    {
        if (GameStatics.gameStart == true)
        {
           // EnemyMapIconsInst(world_Enemy_transform.Count);
            //Debug.Log("imageTransform_Enemy[i].)
            // Update the anchored position of the imageTransform.
            if (worldT_Player_transform != null)
                imageTransform_Plaeyr.anchoredPosition = -FindInterfacePoint(worldT_Player_transform);

            EnemyAnchorPositonUpdate(world_Enemy_transform);
            /*  if(world_Enemy_transform != null)
              imageTransform_Enemy.anchoredPosition = FindInterfacePoint(world_Enemy_transform);*/
        }



    }

    /// <summary>
    /// Finds the point on the UI map corresponding to a world position.
    /// </summary>
    /// <returns>A Vector2 position in the local space of the map's RectTransform.</returns>
    private Vector2 FindInterfacePoint(Transform _WorldTrnsform)
    {
        // Get the normalized position in the world bounds.
        Vector2 normalizedPosition = bounds.FindNormalizedPosition(_WorldTrnsform.position);

        // Convert the normalized position to a local position on the map.
        Vector2 localPoint = Rect.NormalizedToPoint(MapTransform.rect, normalizedPosition);

        // Adjust the local point to consider the MapBG structure.
        localPoint.x = Mathf.Lerp(0, MapTransform.rect.width, normalizedPosition.x) - MapTransform.rect.width / 2;
       
        localPoint.y = Mathf.Lerp(0, MapTransform.rect.height, normalizedPosition.y) - MapTransform.rect.height / 2;

        // Return the adjusted local point.
        return localPoint;
    }
}