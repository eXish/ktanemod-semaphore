using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SemaphoreSequencer : MonoBehaviour
{
    private static readonly Dictionary<char, SemaphoreCharacter> CharacterMapping = new Dictionary<char, SemaphoreCharacter>()
    {
        { '0', SemaphoreCharacter.Zero },
        { '1', SemaphoreCharacter.One },
        { '2', SemaphoreCharacter.Two },
        { '3', SemaphoreCharacter.Three },
        { '4', SemaphoreCharacter.Four },
        { '5', SemaphoreCharacter.Five },
        { '6', SemaphoreCharacter.Six },
        { '7', SemaphoreCharacter.Seven },
        { '8', SemaphoreCharacter.Eight },
        { '9', SemaphoreCharacter.Nine },
        { 'A', SemaphoreCharacter.A },
        { 'B', SemaphoreCharacter.B },
        { 'C', SemaphoreCharacter.C },
        { 'D', SemaphoreCharacter.D },
        { 'E', SemaphoreCharacter.E },
        { 'F', SemaphoreCharacter.F },
        { 'G', SemaphoreCharacter.G },
        { 'H', SemaphoreCharacter.H },
        { 'I', SemaphoreCharacter.I },
        { 'J', SemaphoreCharacter.J },
        { 'K', SemaphoreCharacter.K },
        { 'L', SemaphoreCharacter.L },
        { 'M', SemaphoreCharacter.M },
        { 'N', SemaphoreCharacter.N },
        { 'O', SemaphoreCharacter.O },
        { 'P', SemaphoreCharacter.P },
        { 'Q', SemaphoreCharacter.Q },
        { 'R', SemaphoreCharacter.R },
        { 'S', SemaphoreCharacter.S },
        { 'T', SemaphoreCharacter.T },
        { 'U', SemaphoreCharacter.U },
        { 'V', SemaphoreCharacter.V },
        { 'W', SemaphoreCharacter.W },
        { 'X', SemaphoreCharacter.X },
        { 'Y', SemaphoreCharacter.Y },
        { 'Z', SemaphoreCharacter.Z }
    };

    public static SemaphoreCharacter GetSemaphoreCharacter(char character)
    {
        return CharacterMapping[character];
    }

    public SemaphoreBody SemaphoreBody = null;

    public List<SemaphoreCharacter> characterSequence = null;
    public int currentCharacterIndex = -1;

    public SemaphoreCharacter CurrentCharacter
    {
        get
        {
            if (currentCharacterIndex == -1)
            {
                return SemaphoreCharacter.Rest_Space;
            }

            return characterSequence[currentCharacterIndex];
        }
    }

    public void SetSequenceString(string sequenceString)
    {
        characterSequence = null;
        SetCharacterIndex(-1);

        sequenceString = Regex.Replace(sequenceString, @"[^A-Z0-9]", "");
        BuildCharacterSequence(sequenceString);
    }

    public void NextCharacter()
    {
        SetCharacterIndex(currentCharacterIndex + 1);
    }

    public void PreviousCharacter()
    {
        SetCharacterIndex(currentCharacterIndex - 1);
    }

    public void Error()
    {
        SemaphoreBody.Character = SemaphoreCharacter.Error;
        StartCoroutine(ReturnToCharacter());
    }

    private void BuildCharacterSequence(string sequenceString)
    {
        bool? isNumerals = null;
        characterSequence = new List<SemaphoreCharacter>();

        foreach (char character in sequenceString)
        {
            bool isNumber = char.IsNumber(character);
            if (isNumber && (!isNumerals.HasValue || !isNumerals.Value))
            {
                isNumerals = true;
                characterSequence.Add(SemaphoreCharacter.Numerals);
            }
            else if (!isNumber && (!isNumerals.HasValue || isNumerals.Value))
            {
                isNumerals = false;
                characterSequence.Add(SemaphoreCharacter.Letters);
            }

            characterSequence.Add(GetSemaphoreCharacter(character));
        }

        SetCharacterIndex(0);
    }

    private IEnumerator ReturnToCharacter()
    {
        yield return new WaitForSeconds(1.0f);
        SetCharacterIndex(currentCharacterIndex);
    }

    private void SetCharacterIndex(int characterIndex)
    {
        if (characterSequence != null && characterIndex >= 0 && characterIndex < characterSequence.Count)
        {
            currentCharacterIndex = characterIndex;
            SemaphoreBody.Character = characterSequence[characterIndex];
        }
        else if (currentCharacterIndex == -1)
        {
            SemaphoreBody.Character = SemaphoreCharacter.Rest_Space;
        }
    }
}
