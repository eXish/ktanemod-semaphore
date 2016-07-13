using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    public KMSelectable Selectable;
    public KMAudio Audio;
    public Animator Animator;

    private void Start()
    {
        Selectable.OnInteract += OnInteract;
    }

    private bool OnInteract()
    {
        Audio.PlaySoundAtTransform("button_press", transform);
        Animator.SetTrigger("ButtonPress");
        return false;
    }
}