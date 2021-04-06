using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedEffect : MonoBehaviour
{
    public SpriteRenderer glowRenderer;
    public ParticleSystem glowParticleSys;

    private void Awake()
    {
        //glowRenderer.sharedMaterial = glowMaterialInstance = new Material(glowRenderer.sharedMaterial);
        SetEffectColour(GetComponentInParent<TileData>().color);
    }

    public void SetEffectColour(ColorPalette.TileColor tileColor)
    {
        Debug.Assert(glowRenderer != null, "Glow renderer is null");
        Debug.Assert(glowParticleSys != null, "Glow particle system is null");

        Color colorRGB = ColorPalette.ConvertTileColourToRGB(tileColor, true);

        //glowMaterialInstance.color = colorRGB;
        glowRenderer.color = colorRGB;
        ParticleSystem.MainModule mainModule = glowParticleSys.main;
        mainModule.startColor = colorRGB;
    }



}
