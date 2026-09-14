using UnityEngine;

public class WaitSecondsPausable : CustomYieldInstruction
{
    private float _remaining;

    public WaitSecondsPausable(float seconds) { _remaining = seconds; }

    public override bool keepWaiting
    {
        get
        {
            if (GamePause.IsPaused) return true;

            _remaining -= Time.deltaTime;
            return _remaining > 0f;
        }
    }
}