using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public SpriteRenderer glowRenderer;
    public ParticleSystem glowParticleSys;

    private void Awake()
    {
        Debug.Assert(glowRenderer != null, "Glow renderer is null");
        Debug.Assert(glowParticleSys != null, "Glow particle system is null");

        SetEffectColour(GetComponentInParent<TileSelectionBehaviour>().type);
    }

    public void SetEffectColour(TileSelectionBehaviour.Type tileType)
    {
        Color colorRGB = GameCoordinator.Instance.TileLoadouts.ConvertTileTypeToRGB(tileType, true);

        glowRenderer.color = colorRGB;
        ParticleSystem.MainModule mainModule = glowParticleSys.main;
        mainModule.startColor = colorRGB;
    }

}
