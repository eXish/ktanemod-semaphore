using System;
using UnityEngine;

public class SemaphoreModule : MonoBehaviour
{
    public KMBombModule BombModule;
    public SemaphoreSequencer Sequencer;

    public KMSelectable PreviousButton;
    public KMSelectable NextButton;
    public KMSelectable OKButton;

    private void Start()
    {
        BombModule.OnActivate += StartModule;
        BombModule.OnDeactivate += EndModule;

        PreviousButton.OnInteract += OnPrevious;
        NextButton.OnInteract += OnNext;
        OKButton.OnInteract += OnOK;
    }

    private void StartModule()
    {
        Sequencer.SetSequenceString("TEST");
    }

    private void EndModule()
    {
        BombModule.OnActivate -= StartModule;
        BombModule.OnDeactivate -= EndModule;
    }

    private void PassModule()
    {
        BombModule.HandlePass();
    }

    private void FailModule()
    {
        Sequencer.Error();
        BombModule.HandleStrike();
    }

    private bool OnPrevious()
    {
        Sequencer.PreviousCharacter();
        return false;
    }

    private bool OnNext()
    {
        Sequencer.NextCharacter();
        return false;
    }

    private bool OnOK()
    {
        //TODO - LOGIC!!
        PassModule();
        return false;
    }
}
