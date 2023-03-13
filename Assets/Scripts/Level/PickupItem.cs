using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] private PickupItemType type;
        [SerializeField] private int playerLayer = 6;
        [SerializeField] private int poleLayer = 7;

        private void Start()
        {
            GameStateMachine.Main.onGameReset += () => gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == playerLayer || other.gameObject.layer == poleLayer) {
                switch (type) {
                    case PickupItemType.Gem:
                        Debug.Log("Picked up gem!");
                        PlayerDataManager.Main.GetGems(1);
                        break;
                    case PickupItemType.PoleUpgrade:
                        Debug.Log("Picked up powerup");
                        PlayerController.Main.PowerUp();
                        break;
                }
                gameObject.SetActive(false);
            }
        }
    }

    public enum PickupItemType {
        Gem, PoleUpgrade
    }
}