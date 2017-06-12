using System.Collections;
using System;
using UnityEngine;

public class GameOverState : StateGeneric
{    
    public override void Enter()
    {
        LevelManager.instance.GetGameOverTxtObject().SetActive(true);
    }

    public override void Execute()
    {
      
    }

    public override void Exit()
    {
        LevelManager.instance.GetGameOverTxtObject().SetActive(false);
    }
}
