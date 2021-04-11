using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatEffect : MonoBehaviour
{
    public SpriteRenderer spriteRen;
    public Animator animator;

    /// <summary>
    /// Check for null components and set the effects colour
    /// </summary>
    private void Awake()
    {
        Debug.Assert(spriteRen != null, "Sprite Renderer is null");
        Debug.Assert(animator != null, "Animator is null");

        SetEffectColour(GetComponentInParent<TileBehaviour>().type);
    }

    /// <summary>
    /// Set the sprites colour
    /// </summary>
    /// <param name="tileType">The TileType to extract the colour from</param>
    public void SetEffectColour(TileType tileColor)
    {
        spriteRen.color = GameCoordinator.Instance.TileData.ConvertTileTypeToRGB(tileColor, true);
    }

    /// <summary>
    /// Trigger the spitesheet animation to play
    /// </summary>
    public void PlaySplatAnimation()
    {
        animator.SetTrigger("Splat");
    }
}
