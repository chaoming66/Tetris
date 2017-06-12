using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StateManager {

    #region Struct

    // Data Structure for transitions
    public class StateTransition : IEquatable<StateTransition>
    {
        public StateGeneric from;
        public StateGeneric to;
        public StateTransition(StateGeneric init, StateGeneric end) { from = init; to = end; }

        public bool Equals(StateTransition other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return from.ToString().Equals(other.from.ToString()) && to.ToString().Equals(other.to.ToString());
        }
    }

    #endregion

    #region Private Variables

    protected StateGeneric m_currentState = null;
    protected StateGeneric m_nextState = null;
    protected StateGeneric m_prevState = null;

    protected Dictionary<string, StateGeneric> m_allStates = new Dictionary<string, StateGeneric>();
    protected List<StateTransition> m_transitions = new List<StateManager.StateTransition>();

    #endregion

    #region Public Interface

    /// <summary>
    /// Called by State Machine when state is entered.
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="state"></param>
    public void RegisterState(string stateName, StateGeneric state)
    {
        state.RegisterManager(this);
        m_allStates.Add(stateName, state);
    }

    /// <summary>
    /// Set the default state of this machine, it would also enter the default state
    /// </summary>
    /// <param name="stateName"></param>
    public void SetDefaultState(string stateName)
    {
        if (m_allStates.ContainsKey(stateName))
        {
            m_currentState = m_allStates[stateName];
        }
        else
        {
            Debug.LogError("State " + stateName + "doesn't exist in the StateMachine");
        }

        m_currentState.Enter();
    }

    /// <summary>
    /// State Transition
    /// </summary>
    /// <param name="newStateName"></param>
    public void SwitchState(string newStateName)
    {
        if (m_currentState != null)
        {
            StateTransition transition = 
                new StateManager.StateTransition(m_currentState, m_allStates[newStateName]);

            // Make sure the transition is valid
            if (m_transitions.Contains(transition))
            {
                m_prevState = m_currentState;
                m_currentState.Exit();
                m_currentState = m_allStates[newStateName];
                m_currentState.Enter();
            }
            else
            {
                Debug.LogError("There is no transition in between " + m_currentState.ToString() + " " + newStateName);
            }
        }
        else
        {
            Debug.LogError("Current State is null!");
        }
    }

    /// <summary>
    /// This will be called one time per frame, driven my a unity mono script
    /// </summary>
    public void Execute()
    {
        if (m_currentState != null)
        {
            m_currentState.Execute();
        }
        else
        {
            Debug.LogError("Current State is null!");
        }
    }

    /// <summary>
    /// Build transitions between states
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void AddTransition(StateGeneric from, StateGeneric to)
    {
        StateTransition transition = new StateTransition(from, to);
        if (!m_transitions.Contains(transition))
        {
            m_transitions.Add(transition);
        }
        else
        {
            Debug.LogError("transition " + from.ToString() + " to " + to.ToString() + " already exists!");
        }
    }

    #endregion
}
