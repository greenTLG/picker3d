using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public static class GameManager
{
    public enum GamePhases
    {
        Start,
        Running,
        Finishing,
        Finished,
        Failed
    }

    static GamePhases gamePhase = GamePhases.Start;

    public static GamePhases GamePhase { get => gamePhase; private set => gamePhase = value; }

    public static void StartToMove()
    {
        gamePhase = GamePhases.Running;
        //UIManager.Instance.StartToMove();
        PlayerMovementController.Instance.StartToMove_Forward();
    }

    public static void StartToFinish()
    {
        if (GamePhase != GamePhases.Running)
            return;
        gamePhase = GamePhases.Finishing;
    }

    public static void PlayerFinished()
    {
        if (GamePhase != GamePhases.Finishing)
            return;
        GamePhase = GamePhases.Finished;
        LevelManager.Instance.NextLevel();
        //Show WinPanel UI
    }

    public static void PlayerFailed()
    {
        if (GamePhase == GamePhases.Finishing)
            PlayerFinished();

        if (GamePhase != GamePhases.Running)
            return;
        GamePhase = GamePhases.Failed;
    }

    public static void OnClickedNextLevelButton()
    {
        PlayerMovementController.Instance.StartToMove_Horizontal();
        GamePhase = GamePhases.Start;
    }

    public static void LevelLoaded()
    {
        PlayerMovementController.Instance.transform.position = LevelManager.Instance.GetCurrentLevel().GetPlayerStartPos();
    }

    public static void ResetGame()
    {


    }

}
