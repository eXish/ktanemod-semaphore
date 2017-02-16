using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
    private void Awake()
    {
        BombModule.GenerateLogFriendlyName();
    }

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

        string[] lines = new string[8] { "", "", "", "", "", "", "", "" };

        for(int characterIndex = 0; characterIndex < Sequencer.characterSequence.Count; ++characterIndex)
        {
            SemaphoreCharacter character = Sequencer.characterSequence[characterIndex];
            SemaphoreCombinationAttribute combination = character.GetAttributeOfType<SemaphoreCombinationAttribute>();
            string[] characterLines = combination.GetStringRepresentation();
            for (int lineIndex = 0; lineIndex < characterLines.Length; ++lineIndex)
            {
                lines[lineIndex] += string.Format("{0} ", characterLines[lineIndex]);
            }

            string characterIdentifier = character.ToString();
            int leftPad = (9 - characterIdentifier.Length) / 2;
            int rightPad = (9 - characterIdentifier.Length) - leftPad;

            lines[6] += string.Format("{0}{1}{2} ", leftPad > 0 ? new string(' ', leftPad) : "", characterIdentifier, rightPad > 0 ? new string(' ', rightPad) : "");
            lines[7] += character == _correctSemaphoreCharacter ? "*ANSWER*  " :  "          ";

            if (characterIndex < Sequencer.characterSequence.Count - 1)
            {
                for (int lineIndex = 0; lineIndex < lines.Length; ++lineIndex)
                {
                    lines[lineIndex] += "¦ ";
                }
            }
        }

        StringBuilder logString = new StringBuilder();
        logString.Append("Module generated with the following semaphore sequence:\n");

        foreach (string line in lines)
        {
            logString.Append(line).Append("\n");
        }

        logString.Append("\n");

        BombModule.Log(logString.ToString());
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
        BombModule.LogFormat("OK button was pressed on semaphore #{0} ({1})", Sequencer.currentCharacterIndex + 1, Sequencer.CurrentCharacter.ToString());

        if (Sequencer.CurrentCharacter == _correctSemaphoreCharacter)
        {
            BombModule.Log("Valid answer! Module defused!");
            BombModule.HandlePass();
        }
        else
        {
            BombModule.Log("Invalid answer! Module triggered a strike!");
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
