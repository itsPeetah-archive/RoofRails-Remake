using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class PlayerInputManager : MonoBehaviour
    {
        private enum PointerInputType { Default, Up, Down }

        private bool swiping = false;
        private Vector2 startingSwipePosition, currentSwipePosition, touchStartingPosition;

        private Vector2 currentPointerPosition;
        private bool touchDetected = false;

        [SerializeField] private float minVerticalSwipePercent = 0.05f;

        private void Update()
        {
            GetPointerInput();

            if (GameStateMachine.Main.State != GameState.Playing) return;

            float dx, dy, px, py;

            if (!swiping && GetPointerInput(PointerInputType.Down))
            {
                swiping = true;
                startingSwipePosition = currentPointerPosition;
                touchStartingPosition = currentPointerPosition;
            }
            else if (swiping && GetPointerInput(PointerInputType.Up))
            {
                swiping = false;
                dx = currentSwipePosition.x - touchStartingPosition.x;
                dy = currentSwipePosition.y - touchStartingPosition.y;

                // sign : left or right
                // magnitude: percent of screen traveled
                px = dx / Screen.width;
                py = dy / Screen.height;

                if (Mathf.Abs(dx) < Mathf.Abs(dy) && Mathf.Abs(py) >= minVerticalSwipePercent && Mathf.Abs(px) < minVerticalSwipePercent)
                {
                    if (py > 0)
                    {
                        Debug.Log("Swipe up!");
                        PlayerController.Main.Jump();
                    }
                    else
                    {
                        Debug.Log("Swipe down!");
                        PlayerController.Main.Slide();
                    }
                }

            }
            else if (swiping && GetPointerInput())
            {
                currentSwipePosition = currentPointerPosition;
                dx = currentSwipePosition.x - startingSwipePosition.x;
                px = dx / Screen.width;

                if (Mathf.Abs(dx) > 0.01)
                    LevelMover.Main.MoveHorizontally(-px); // negative cuz we're moving the level not the player
                startingSwipePosition = currentSwipePosition;
            }
        }

        private bool GetPointerInput(PointerInputType mode = PointerInputType.Default)
        {

#if UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount < 1) return false;
            Touch touch = Input.GetTouch(0);
            currentPointerPosition = touch.position;
            switch (mode)
            {
                case PointerInputType.Up:
                    return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
                case PointerInputType.Down:
                    return touch.phase == TouchPhase.Began;
                default:
                    return touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary ;
            }
#else
            currentPointerPosition = Input.mousePosition;
            switch (mode)
            {
                case PointerInputType.Up:
                    return Input.GetMouseButtonUp(0);
                case PointerInputType.Down:
                    return Input.GetMouseButtonDown(0);
                default:
                    return Input.GetMouseButton(0);
            }
#endif
        }
    }
}