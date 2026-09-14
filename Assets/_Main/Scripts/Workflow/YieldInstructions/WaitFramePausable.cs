using UnityEngine;

public class WaitFramePausable : CustomYieldInstruction
{
    public override bool keepWaiting => GamePause.IsPaused;
}