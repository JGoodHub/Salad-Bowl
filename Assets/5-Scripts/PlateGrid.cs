using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGrid : MonoBehaviour
{
    public Transform plateParent;
    private GameObject[,] plateObjects;

    private void Start()
    {
        int width = GameCoordinator.Instance.BoardLayout.width;
        int height = GameCoordinator.Instance.BoardLayout.height;
        float spacing = GameCoordinator.Instance.BoardLayout.spacing;

        plateObjects = new GameObject[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject plateObject = Instantiate(GameCoordinator.Instance.BoardLayout.platePrefab, Vector3.zero, Quaternion.identity, plateParent);
                plateObject.transform.localPosition = new Vector3(x - (width / 2) + (width % 2 == 0 ? 0.5f : 0), y - (height / 2) + (height % 2 == 0 ? 0.5f : 0), 0f);
                plateObject.transform.localPosition *= spacing;

                plateObjects[x, y] = plateObject;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (GameCoordinator.Instance.BoardLayout == null)
            return;

        int width = GameCoordinator.Instance.BoardLayout.width;
        int height = GameCoordinator.Instance.BoardLayout.height;
        float spacing = GameCoordinator.Instance.BoardLayout.spacing;

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
