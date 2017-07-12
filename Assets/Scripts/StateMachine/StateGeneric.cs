using System.Collections;

public abstract class StateGeneric
{
    protected StateManager m_manager = null;

    public void RegisterManager(StateManager manager)
    {
        m_manager = manager;
    }

    /// <summary>
    /// Called by State Machine when state is entered.
    /// </summary>  
    public abstract void Enter();

    /// <summary>
    //  Execute is called once per frame
    /// </summary>  
    public abstract void Execute();

    /// <summary>
    /// Called by State Machine when state is exited.
    /// </summary>
    public abstract void Exit();
}
