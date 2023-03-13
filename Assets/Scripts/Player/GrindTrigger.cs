using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class GrindTrigger : MonoBehaviour
    {
        public bool grinding;
        [SerializeField] private int railLayer = 9;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == railLayer)
            {
                grinding = true;
                PlayerController.Main.StartGrinding();
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == railLayer)
            {
                grinding = false;
                PlayerController.Main.StopGrinding();
            }
        }
    }
}