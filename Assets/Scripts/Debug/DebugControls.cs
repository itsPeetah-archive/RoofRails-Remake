using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RoofRails;

public class DebugControls : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameStateMachine.Main.TriggerState(GameState.Menu);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            GameStateMachine.Main.TriggerState(GameState.Playing);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameStateMachine.Main.TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameStateMachine.Main.TriggerState(GameState.GameLost);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameStateMachine.Main.TriggerState(GameState.GameWon);
        }
    }
}
