using UnityEngine;

public class IdleState : IShipState
{
    public const string IS_FIRING = "isFiring";
    public void HandleInput(PlayerController context)
    {
        if (Input.GetMouseButton(0)) 
        {
            context.SetState(new FiringState());
        }
    }

    public void UpdateState(PlayerController context)
    {
        context.playAnim.SetBool(IS_FIRING, false);
    }
}
