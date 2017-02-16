using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SemaphoreCombinationAttribute : System.Attribute
{
    public SemaphoreCombinationAttribute(FlagOrientation leftHand, FlagOrientation rightHand)
    {
        LeftHand = leftHand;
        RightHand = rightHand;
    }

    public string[] GetStringRepresentation()
    {
        char[][] lines = new char[][] { "         ".ToCharArray(),
                                        "         ".ToCharArray(),
                                        "  +   +  ".ToCharArray(),
                                        "         ".ToCharArray(),
                                        "         ".ToCharArray() };

        string leftHandCardinal = "";
        switch (LeftHand)
        {
            case FlagOrientation.UpIn:
                leftHandCardinal = "NE";
                lines[0][4] = '/';
                lines[1][3] = '/';
                break;
            case FlagOrientation.Up:
                leftHandCardinal = "N";
                lines[0][2] = '|';
                lines[1][2] = '|';
                break;
            case FlagOrientation.UpOut:
                leftHandCardinal = "NW";
                lines[0][0] = '\\';
                lines[1][1] = '\\';
                break;
            case FlagOrientation.Out:
                leftHandCardinal = "W";
                lines[2][0] = '-';
                lines[2][1] = '-';
                break;
            case FlagOrientation.DownOut:
                leftHandCardinal = "SW";
                lines[3][1] = '/';
                lines[4][0] = '/';
                break;
            case FlagOrientation.Down:
                leftHandCardinal = "S";
                lines[3][2] = '|';
                lines[4][2] = '|';
                break;
            case FlagOrientation.DownIn:
                leftHandCardinal = "SE";
                lines[3][3] = '\\';
                lines[4][4] = '\\';
                break;
            default:
                break;
        }

        string rightHandCardinal = "";
        switch (RightHand)
        {
            case FlagOrientation.UpIn:
                rightHandCardinal = "NW";
                lines[0][4] = '\\';
                lines[1][5] = '\\';
                break;
            case FlagOrientation.Up:
                rightHandCardinal = "N";
                lines[0][6] = '|';
                lines[1][6] = '|';
                break;
            case FlagOrientation.UpOut:
                rightHandCardinal = "NE";
                lines[0][8] = '/';
                lines[1][7] = '/';
                break;
            case FlagOrientation.Out:
                rightHandCardinal = "E";
                lines[2][7] = '-';
                lines[2][8] = '-';
                break;
            case FlagOrientation.DownOut:
                rightHandCardinal = "SE";
                lines[3][7] = '\\';
                lines[4][8] = '\\';
                break;
            case FlagOrientation.Down:
                rightHandCardinal = "S";
                lines[3][6] = '|';
                lines[4][6] = '|';
                break;
            case FlagOrientation.DownIn:
                rightHandCardinal = "SW";
                lines[3][5] = '/';
                lines[4][4] = '/';
                break;
            default:
                break;
        }

        List<string> stringLines = lines.Select((x) => new string(x)).ToList();
        stringLines.Add(string.Format(" [{0,2}.{1,-2}] ", leftHandCardinal, rightHandCardinal));

        return stringLines.ToArray();
    }

    public readonly FlagOrientation LeftHand;
    public readonly FlagOrientation RightHand;
}

public enum SemaphoreCharacter
{
    [SemaphoreCombination(FlagOrientation.DownIn, FlagOrientation.DownIn)]
    Rest_Space,
    [SemaphoreCombination(FlagOrientation.Up, FlagOrientation.UpOut)]
    Numerals,
    [SemaphoreCombination(FlagOrientation.Up, FlagOrientation.Out)]
    Letters,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.Up)]
    Zero,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.Down)]
    One,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.Down)]
    Two,
    [SemaphoreCombination(FlagOrientation.UpOut, FlagOrientation.Down)]
    Three,
    [SemaphoreCombination(FlagOrientation.Up, FlagOrientation.Down)]
    Four,
    [SemaphoreCombination(FlagOrientation.Down, FlagOrientation.UpOut)]
    Five,
    [SemaphoreCombination(FlagOrientation.Down, FlagOrientation.Out)]
    Six,
    [SemaphoreCombination(FlagOrientation.Down, FlagOrientation.DownOut)]
    Seven,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.DownIn)]
    Eight,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.UpIn)]
    Nine,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.Down)]
    A,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.Down)]
    B,
    [SemaphoreCombination(FlagOrientation.UpOut, FlagOrientation.Down)]
    C,
    [SemaphoreCombination(FlagOrientation.Up, FlagOrientation.Down)]
    D,
    [SemaphoreCombination(FlagOrientation.Down, FlagOrientation.UpOut)]
    E,
    [SemaphoreCombination(FlagOrientation.Down, FlagOrientation.Out)]
    F,
    [SemaphoreCombination(FlagOrientation.Down, FlagOrientation.DownOut)]
    G,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.DownIn)]
    H,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.UpIn)]
    I,
    [SemaphoreCombination(FlagOrientation.Up, FlagOrientation.Out)]
    J,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.Up)]
    K,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.UpOut)]
    L,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.Out)]
    M,
    [SemaphoreCombination(FlagOrientation.DownOut, FlagOrientation.DownOut)]
    N,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.UpIn)]
    O,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.Up)]
    P,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.UpOut)]
    Q,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.Out)]
    R,
    [SemaphoreCombination(FlagOrientation.Out, FlagOrientation.DownOut)]
    S,
    [SemaphoreCombination(FlagOrientation.UpOut, FlagOrientation.Up)]
    T,
    [SemaphoreCombination(FlagOrientation.UpOut, FlagOrientation.UpOut)]
    U,
    [SemaphoreCombination(FlagOrientation.Up, FlagOrientation.DownOut)]
    V,
    [SemaphoreCombination(FlagOrientation.UpIn, FlagOrientation.Out)]
    W,
    [SemaphoreCombination(FlagOrientation.UpIn, FlagOrientation.DownOut)]
    X,
    [SemaphoreCombination(FlagOrientation.UpOut, FlagOrientation.Out)]
    Y,
    [SemaphoreCombination(FlagOrientation.DownIn, FlagOrientation.Out)]
    Z,
    [SemaphoreCombination(FlagOrientation.UpOut, FlagOrientation.DownOut)]
    Cancel,
    [SemaphoreCombination(FlagOrientation.Error, FlagOrientation.Error)]
    Error,
}

public class SemaphoreBody : MonoBehaviour
{
    public SemaphoreFlag LeftFlag;
    public SemaphoreFlag RightFlag;

    public SemaphoreCharacter Character = SemaphoreCharacter.Rest_Space;
    private SemaphoreCharacter _currentCharacter = SemaphoreCharacter.Rest_Space;

    private void Start()
    {
        ChangeCharacter(Character);
    }

    private void Update()
    {
        if (Character != _currentCharacter)
        {
            ChangeCharacter(Character);
        }
    }

    private void ChangeCharacter(SemaphoreCharacter requestedCharacter)
    {
        SemaphoreCombinationAttribute combination = requestedCharacter.GetAttributeOfType<SemaphoreCombinationAttribute>();
        LeftFlag.Orientation = combination.LeftHand;
        RightFlag.Orientation = combination.RightHand;
        _currentCharacter = requestedCharacter;
    }
}
