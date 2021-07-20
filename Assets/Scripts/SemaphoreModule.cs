using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Collections;
using System.Text.RegularExpressions;

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

    #region Twitch Plays
    public string TwitchHelpMessage = "Cycle the flags with !{0} cycle. Set the flag at a given position with !{0} set 5 (1 is the first flag). Move to the next flag with !{0} move right or !{0} press right. Move to previous flag with !{0} move left or !{0} press left. Submit with !{0} press ok.";

    public IEnumerator ProcessTwitchCommand(string command)
    {
        if (command.Equals("press left", StringComparison.InvariantCultureIgnoreCase) ||
            command.Equals("move left", StringComparison.InvariantCultureIgnoreCase))
        {
            yield return null;
            yield return new KMSelectable[] { PreviousButton };
        }
        else if (command.Equals("press right", StringComparison.InvariantCultureIgnoreCase) ||
            command.Equals("move right", StringComparison.InvariantCultureIgnoreCase))
        {
            yield return null;
            yield return new KMSelectable[] { NextButton };
        }
        else if (command.Equals("press ok", StringComparison.InvariantCultureIgnoreCase))
        {
            yield return null;
            yield return new KMSelectable[] { OKButton };
        }
        else if (command.Equals("cycle", StringComparison.InvariantCultureIgnoreCase))
        {
            yield return null;
            int initialCharacterIndex = Sequencer.currentCharacterIndex;

            while (Sequencer.currentCharacterIndex > 0)
            {
                yield return PreviousButton;
                yield return PreviousButton;
                yield return new WaitForSeconds(0.1f);
            }

            while (Sequencer.currentCharacterIndex < Sequencer.characterSequence.Count - 1)
            {
                yield return new WaitForSeconds(1.5f);
                yield return NextButton;
                yield return NextButton;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(3.0f);

            while (Sequencer.currentCharacterIndex > initialCharacterIndex)
            {
                yield return PreviousButton;
                yield return PreviousButton;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            Match setMatch = Regex.Match(command, "^set ([0-9]+)$", RegexOptions.IgnoreCase);
            if (!setMatch.Success)
            {
                yield break;
            }

            string indexString = setMatch.Groups[1].Value;
            int index = int.MinValue;
            if (int.TryParse(indexString, out index) && index >= 1 && index <= Sequencer.characterSequence.Count)
            {
                yield return null;
                index--;
                while (Sequencer.currentCharacterIndex > index)
                {
                    yield return PreviousButton;
                    yield return PreviousButton;
                    yield return new WaitForSeconds(0.1f);
                }
                while (Sequencer.currentCharacterIndex < index)
                {
                    yield return NextButton;
                    yield return NextButton;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                yield break;
            }
        }
    }

    public IEnumerator TwitchHandleForcedSolve()
    {
        while (_semaphoreSequence == null) yield return true;
        int endIndex = Sequencer.characterSequence.IndexOf(_correctSemaphoreCharacter);
        int curIndex = Sequencer.currentCharacterIndex;
        if (curIndex < endIndex)
        {
            for (int i = 0; i < endIndex - curIndex; i++)
            {
                NextButton.OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (curIndex > endIndex)
        {
            for (int i = 0; i < curIndex - endIndex; i++)
            {
                PreviousButton.OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
        OKButton.OnInteract();
    }
    #endregion
}
