public interface IShipState
{
    void HandleInput(PlayerController context);
    void UpdateState(PlayerController context);
}
