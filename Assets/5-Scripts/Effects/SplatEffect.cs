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

        SetEffectColour(GetComponentInParent<Tile>().type);
    }

    public void SetEffectColour(Tile.Type tileColor)
    {
        spriteRen.color = ColorPalette.Instance.ConvertTileTypeToRGB(tileColor, true);
    }

    public void PlaySplatAnimation()
    {
        animator.SetTrigger("SplatTrigger");
    }
}
