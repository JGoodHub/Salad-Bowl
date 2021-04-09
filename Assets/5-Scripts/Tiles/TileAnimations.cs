using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TileAnimations : MonoBehaviour
{
    public Animator animator;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayShrinkTileAnimation()
    {
        animator.SetTrigger("ShrinkTrigger");
    }





}
