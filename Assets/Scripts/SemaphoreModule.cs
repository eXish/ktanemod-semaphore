using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreModule : MonoBehaviour
{
    #region Constants
    private static readonly string VALID_CHARACTERS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int MAXIMUM_PICK_COUNT = 5;
    #endregion

    #region Public Fields
    public KMBombModule BombModule;
    public KMBombInfo BombInfo;
    public SemaphoreSequencer Sequencer;

    public KMSelectable PreviousButton;
    public KMSelectable NextButton;
    public KMSelectable OKButton;
    #endregion

    #region Private Fields
    private string _semaphoreSequence = null;
    private SemaphoreCharacter _correctSemaphoreCharacter = SemaphoreCharacter.Cancel;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        BombModule.OnActivate += StartModule;

        PreviousButton.OnInteract += OnPrevious;
        NextButton.OnInteract += OnNext;
        OKButton.OnInteract += OnOK;
    }

    private void OnDestroy()
    {
        EndModule();
    }
    #endregion

    #region Module Lifecycle
    private void StartModule()
    {
        _semaphoreSequence = GenerateSemaphoreSequence();
        Sequencer.SetSequenceString(_semaphoreSequence);
    }

    private void EndModule()
    {
        BombModule.OnActivate -= StartModule;
    }
    #endregion

    #region Button Logic
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
        if (Sequencer.CurrentCharacter == _correctSemaphoreCharacter)
        {
            BombModule.HandlePass();
        }
        else
        {
            Sequencer.Error();
            BombModule.HandleStrike();
        }

        return false;
    }
    #endregion

    #region Pass/Fail Generation
    private string GenerateSemaphoreSequence()
    {
        string serialNumber = BombInfo.GetSerialNumber();
        List<char> pickedCharacters = serialNumber.Distinct().RandomPick(MAXIMUM_PICK_COUNT, true).ToList();
        char correctCharacter = VALID_CHARACTERS.Except(serialNumber).RandomPick(1, true).First();

        _correctSemaphoreCharacter = SemaphoreSequencer.GetSemaphoreCharacter(correctCharacter);
        pickedCharacters.Add(correctCharacter);
        
        IEnumerable<char> finalSequence = pickedCharacters.Shuffle();
        return new string(finalSequence.ToArray());
    }
    #endregion
}
