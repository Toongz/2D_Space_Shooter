using UnityEngine;

public class FiringState : IShipState
{
    public void HandleInput(PlayerController context)
    {
        if (!Input.GetMouseButton(0)) 
        {
            context.SetState(new IdleState());
        }
    }

    public void UpdateState(PlayerController context)
    {
        context.playAnim.SetBool("isFiring", true);
        context.Shoot();
    }
}
