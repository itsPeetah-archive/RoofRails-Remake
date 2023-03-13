using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class LevelMover : MonoBehaviour
    {
        private static LevelMover main;
        public static LevelMover Main => main;

        [SerializeField] private Transform levelTransform;
        private Vector3 startingPosition;

        [Header("Forward")]
        [SerializeField] private float movingSpeed = 5f;
        [SerializeField] private Vector3 movingDirection = new Vector3(0, 0, -1);

        [Header("Horizontal")]
        [SerializeField] private Vector3 horizontalAxis = new Vector3(1, 0, 0);
        [SerializeField] private float xMin;
        [SerializeField] private float xMax;
        [SerializeField, Range(0, 1)] private float xSensitivity = 1f;
        private float currentX;
        private float deltaX;

        private void Awake()
        {
            main = this;
        }

        private void Start()
        {
            // Sanitize inspector values
            movingSpeed = Mathf.Abs(movingSpeed);
            movingDirection = movingDirection.normalized;

            // Horizontal Movement
            if (xMin > xMax) {
                float temp = xMin;
                xMin = xMax;
                xMax = temp;
            }
            currentX = transform.position.x;
            deltaX = xMax - xMin;

            startingPosition = levelTransform.position;
            GameStateMachine.Main.onGameReset += ResetPosition;
        }

        private void Update()
        {
            if (GameStateMachine.Main.State != GameState.Playing) return;

            levelTransform.Translate((movingSpeed * Time.deltaTime) * movingDirection);
        }

        public void MoveHorizontally(float swipeStrentgh) {

            float dx = deltaX * swipeStrentgh * xSensitivity;
            currentX = Mathf.Clamp(currentX + dx, xMin, xMax);
            levelTransform.transform.position = new Vector3(currentX, 0, levelTransform.position.z);
        }

        private void ResetPosition()
        {
            levelTransform.position = startingPosition;
        }
    }
}