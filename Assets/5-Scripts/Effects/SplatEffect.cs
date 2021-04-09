using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatEffect : MonoBehaviour
{
    public SpriteRenderer spriteRen;
    public Animator animator;

    private void Awake()
    {
        Debug.Assert(spriteRen != null, "Sprite Renderer is null");
        Debug.Assert(animator != null, "Animator is null");

        SetEffectColour(GetComponentInParent<TileSelectionBehaviour>().type);
    }

    public void SetEffectColour(TileType tileColor)
    {
        spriteRen.color = GameCoordinator.Instance.TileLoadouts.ConvertTileTypeToRGB(tileColor, true);
    }

    public void PlaySplatAnimation()
    {
        animator.SetTrigger("SplatTrigger");
    }
}
