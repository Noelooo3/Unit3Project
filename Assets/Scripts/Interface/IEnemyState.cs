public interface IEnemyState
{
    void EnterState(EnemyController controller);
    void UpdateState();
    void ExitState();
}
