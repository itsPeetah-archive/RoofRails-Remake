using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class GrindLoseArea : MonoBehaviour
    {
        [SerializeField] private int playerLayer = 6;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != playerLayer) return;
            GameStateMachine.Main.TriggerState(GameState.GameLost);
            // TODO Make player fall
        }
    }
}