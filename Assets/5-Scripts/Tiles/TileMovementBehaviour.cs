using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileSelectionBehaviour))]
public class TileMovementBehaviour : MonoBehaviour
{
    public TileBehaviour ParentBehaviour { get => GetComponent<TileBehaviour>(); }

    public float averageSpeed;
    public AnimationCurve traversalCurve;

    public bool Moving { get; private set; }
    public Vector2Int GridRef { get; private set; }

    public UnityTileEvent OnTileStartedMoving;
    public UnityTileEvent OnTileFinishedMoving;

    private void Awake()
    {
        GridRef = Vector2Int.one * -1;
    }

    public void MoveToGridRef(int x, int y, bool instant)
    {
        Vector2Int newGridRef = new Vector2Int(x, y);

        if (GridRef.Equals(newGridRef))
            return;

        if (instant)
        {
            transform.position = TileGridManager.Instance.GridToWorldSpace(x, y, transform.parent.position.z);
            GridRef = new Vector2Int(x, y);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MoveToGridPositionCoroutine(newGridRef));
        }
    }

    private IEnumerator MoveToGridPositionCoroutine(Vector2Int newGridRef)
    {
        Moving = true;
        OnTileStartedMoving?.Invoke(ParentBehaviour);

        Vector3 sourcePosition = TileGridManager.Instance.GridToWorldSpace(GridRef.x, GridRef.y, transform.parent.position.z);
        Vector3 targetPosition = TileGridManager.Instance.GridToWorldSpace(newGridRef.x, newGridRef.y, transform.parent.position.z);

        float distance = Vector3.Distance(sourcePosition, targetPosition);
        float traversalTime = distance / averageSpeed;

        float t = 0;
        while (t < traversalTime)
        {
            transform.position = LerpUnclamped(sourcePosition, targetPosition, traversalCurve.Evaluate(t / traversalTime));
            t += Time.deltaTime;
            yield return null;
        }

        Moving = false;
        GridRef = newGridRef;
        transform.position = targetPosition;

        OnTileFinishedMoving?.Invoke(ParentBehaviour);
    }

    public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
    {
        return a + ((b - a) * t);
    }

}
