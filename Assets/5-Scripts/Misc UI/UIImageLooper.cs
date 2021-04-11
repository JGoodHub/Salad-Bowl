using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageLooper : MonoBehaviour
{
    private RectTransform rectTransform;

    public float despawnBoundY;
    public float spawnBoundY;

    public float fallSpeed;
    public float rotateSpeedMin;
    public float rotateSpeedMax;
    private float rotateSpeed;

    private void Start()
    {
        rectTransform = transform as RectTransform;

        rectTransform.anchorMin = new Vector2(0.5f, 0f);
        rectTransform.anchorMax = new Vector2(0.5f, 0f);

        rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax);
    }

    /// <summary>
    /// Translate and rotate the transform
    /// </summary>
    private void Update()
    {
        rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
        rectTransform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        // Wrap the position of the transform if its beyond the despawn boundary

        if (rectTransform.anchoredPosition.y <= despawnBoundY)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, spawnBoundY);
            rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax);
        }
    }


}
