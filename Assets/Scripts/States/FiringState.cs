using UnityEngine;

public class FiringState : IShipState
{
    public const string IS_FIRING = "isFiring";
    public void HandleInput(PlayerController context)
    {
        if (!Input.GetMouseButton(0)) 
        {
            context.SetState(new IdleState());
        }
    }

    public void UpdateState(PlayerController context)
    {
        context.playAnim.SetBool(IS_FIRING, true);
        context.Shoot();
    }
}
