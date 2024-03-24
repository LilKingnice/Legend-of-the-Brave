public abstract class BaseState
{
    protected Enemy currentEnemy;

    public abstract void OnEnter();
    public abstract void OnLogicUpdate();
    public abstract void OnPhysicsCheck();
    public abstract void OnExit();
    
    

}
