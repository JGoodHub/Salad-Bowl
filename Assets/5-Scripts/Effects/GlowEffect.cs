using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public SpriteRenderer glowRenderer;
    public ParticleSystem glowParticleSys;

    /// <summary>
    /// Check for null components and set the effects colour
    /// </summary>
    private void Awake()
    {
        Debug.Assert(glowRenderer != null, "Glow renderer is null");
        Debug.Assert(glowParticleSys != null, "Glow particle system is null");

        SetEffectColour(GetComponentInParent<TileBehaviour>().type);
    }

    /// <summary>
    /// Set the particle system colour
    /// </summary>
    /// <param name="tileType">The TileType to extract the colour from</param>
    public void SetEffectColour(TileType tileType)
    {
        Color colorRGB = GameCoordinator.Instance.TileData.ConvertTileTypeToRGB(tileType, true);

        glowRenderer.color = colorRGB;
        ParticleSystem.MainModule mainModule = glowParticleSys.main;
        mainModule.startColor = colorRGB;
    }

}
