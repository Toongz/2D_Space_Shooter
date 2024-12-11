using UnityEngine;

public class IdleState : IShipState
{
    public void HandleInput(PlayerController context)
    {
        if (Input.GetMouseButton(0)) 
        {
            context.SetState(new FiringState());
        }
    }

    public void UpdateState(PlayerController context)
    {
        context.playAnim.SetBool("isFiring", false);
    }
}
