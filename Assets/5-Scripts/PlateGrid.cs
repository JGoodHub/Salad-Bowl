using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGrid : MonoBehaviour
{

    [Header("Board Parameters")]

    public int width;
    public int height;
    public float spacing;

    public GameObject platePrefab;
    public Transform plateParent;
    private GameObject[,] plateObjects;

    private void OnValidate()
    {
        width = Mathf.Clamp(width, 3, 9);
        height = Mathf.Clamp(height, 3, 9);

        spacing = Mathf.Clamp(spacing, 0f, float.MaxValue);
    }


    private void Start()
    {
        plateObjects = new GameObject[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject plateObject = Instantiate(platePrefab, Vector3.zero, Quaternion.identity, plateParent);
                plateObject.transform.localPosition = new Vector3(x - (width / 2) + (width % 2 == 0 ? 0.5f : 0), y - (height / 2) + (height % 2 == 0 ? 0.5f : 0), 0f);
                plateObject.transform.localPosition *= spacing;

                plateObjects[x, y] = plateObject;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                Vector3 gizmoCente = plateParent.transform.position;
                gizmoCente += new Vector3(x - (width / 2) + (width % 2 == 0 ? 0.5f : 0), y - (height / 2) + (height % 2 == 0 ? 0.5f : 0), 0f) * spacing;

                Gizmos.DrawWireCube(gizmoCente, new Vector3(spacing, spacing, 0));
            }
        }
    }


}
