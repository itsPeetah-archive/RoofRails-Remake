using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private Transform t; // cached transform
        private Vector3 offset;

        private void Start()
        {
            t = transform;
            offset = target.position - t.position;
        }

        private void LateUpdate()
        {
            if (GameStateMachine.Main.GameOver) return;
            t.position = Vector3.Lerp(t.position, target.position - offset, 0.8f);
        }
    }
}