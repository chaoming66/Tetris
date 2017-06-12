using UnityEngine;
using System.Collections;

public class InGameState : StateGeneric
{
    public override void Enter()
    {
		Board.instance.InitializeBoard ();

        Spawner.instance.CreatePiecePatterns();
        Spawner.instance.SpawnNewPiece();

        LevelManager.instance.GetPauseButton().SetActive(true);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        LevelManager.instance.GetPauseButton().SetActive(false);
    }
}