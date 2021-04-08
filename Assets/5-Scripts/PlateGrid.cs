using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlateGrid : MonoBehaviour
{
    public Transform plateParent;
    private GameObject[,] plateObjects;

    private BoardLayoutData boardLayout;

    private void Start()
    {
        boardLayout = GameCoordinator.Instance.BoardLayout;

        plateObjects = new GameObject[boardLayout.width, boardLayout.height];

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                GameObject plateObject = Instantiate(GameCoordinator.Instance.BoardLayout.platePrefab, Vector3.zero, Quaternion.identity, plateParent);
                plateObject.transform.localPosition = new Vector3(x - (boardLayout.width / 2) + (boardLayout.width % 2 == 0 ? 0.5f : 0), y - (boardLayout.height / 2) + (boardLayout.height % 2 == 0 ? 0.5f : 0), 0f);
                plateObject.transform.localPosition *= boardLayout.spacing;

                plateObjects[x, y] = plateObject;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Assert.IsNotNull(FindObjectOfType<GameCoordinator>());

        boardLayout = FindObjectOfType<GameCoordinator>().BoardLayout;

        Gizmos.color = Color.green;

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                Vector3 gizmoCente = plateParent.transform.position;
                gizmoCente += new Vector3(x - (boardLayout.width / 2) + (boardLayout.width % 2 == 0 ? 0.5f : 0), y - (boardLayout.height / 2) + (boardLayout.height % 2 == 0 ? 0.5f : 0), 0f) * boardLayout.spacing;

                Gizmos.DrawWireCube(gizmoCente, new Vector3(boardLayout.spacing, boardLayout.spacing, 0));
            }
        }
    }


}
