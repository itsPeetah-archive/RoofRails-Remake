using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class WinZone : MonoBehaviour
    {
        [SerializeField] private int gemMultiplier;
        [SerializeField] private int playerLayer = 6;

        private void OnTriggerEnter(Collider other)
        {
            PlayerDataManager.Main.levelGemMultiplier = gemMultiplier;
            GameStateMachine.Main.TriggerState(GameState.GameWon);
        }
    }
}