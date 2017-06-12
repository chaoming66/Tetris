using System.Collections;
using System;
using UnityEngine;

public class IntroState : StateGeneric
{
    public override void Enter()
    {
        LevelManager.instance.GetStartPauseButton().SetActive(true);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        LevelManager.instance.GetStartPauseButton().SetActive(false);
    }
}
