using UnityEngine;

public enum FlagOrientation
{
    UpIn,
    Up,
    UpOut,
    Out,
    DownOut,
    Down,
    DownIn,
    Error
}

public class SemaphoreFlag : MonoBehaviour
{
    private Animator _flagAnimator = null;

    public FlagOrientation Orientation = FlagOrientation.Up;
    private FlagOrientation _currentOrientation = FlagOrientation.Up;

    private void Start()
    {
        _flagAnimator = GetComponent<Animator>();
        ChangeOrientation(Orientation);
    }

    private void Update()
    {
        if (Orientation != _currentOrientation)
        {
            ChangeOrientation(Orientation);
        }
    }

    private void ChangeOrientation(FlagOrientation requestedOrientation)
    {
        _flagAnimator.SetTrigger(requestedOrientation.ToString());
        _currentOrientation = requestedOrientation;
    }
}
