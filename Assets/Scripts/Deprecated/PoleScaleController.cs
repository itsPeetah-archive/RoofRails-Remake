using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoofRails
{
    public class PoleScaleController : MonoBehaviour
    {
        [SerializeField] private PoleMaterialController materialController;
        [SerializeField] private float growRatio = 0.2f;
        [SerializeField] private float shrinkRatio = 0.1f;

        private float startingScaleX = 1;
        private float growIncrement;
        private float shrinkDecrement;
        private Vector3 scale;

        private void Start()
        {
            scale = transform.localScale;
            startingScaleX = scale.x;

            growIncrement = growRatio * startingScaleX;
            shrinkDecrement = shrinkRatio * startingScaleX;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Grow();
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Shrink();
            }
        }

        private void Grow() {

            materialController.DoFlashEffect();
            scale.x += growIncrement;
            transform.localScale = scale;
        }

        private void Shrink()
        {

            materialController.DoFlashEffect();
            scale.x = Mathf.Max(0.05f, scale.x - shrinkDecrement);
            transform.localScale = scale;

            if (scale.x <= 0.05f) {
                Debug.Log("Game over: you lose!");
                GameStateMachine.Main.TriggerState(GameState.GameLost);
            }
        }

    }
}