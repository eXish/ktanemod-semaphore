using UnityEngine;

public class SemaphoreCombinationAttribute : System.Attribute
{
    public SemaphoreCombinationAttribute(FlagOrientation leftHand, FlagOrientation rightHand)
    {
        LeftHand = leftHand;
        RightHand = rightHand;
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
