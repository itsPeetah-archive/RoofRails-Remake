using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class DamagingObstacle : MonoBehaviour
    {
        [Header("Damaging options")]
        [SerializeField] private bool killsPlayer = false;
        [SerializeField] private bool triggersInvicibility = false;
        [SerializeField] private bool hitsOnce = false;
        [SerializeField] private float damageInterval = 0.15f;
        [SerializeField] private bool damageOnImpactPoint = false;

        [Header("Physics settings")]
        [SerializeField] private int playerLayer = 6;
        [SerializeField] private int poleLayer = 7;

        private float damageTimer = 0;
        private bool playerDetected = false;

        private void Update()
        {
            if (damageInterval <= 0 || !playerDetected || hitsOnce) return;

            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0;
                PlayerController.Main.TakeDamage();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            playerDetected = true;
            damageTimer = 0;

            bool shouldDamage = false, shouldKill = false;

            if (other.gameObject.layer == playerLayer)
            {
                shouldDamage = damageInterval <= 0;
                shouldKill = killsPlayer;
            }
            else if (other.gameObject.layer == poleLayer)
                shouldDamage = damageInterval <= 0;

            if (shouldDamage)
                PlayerController.Main.TakeDamage(
                    triggerInvincible: triggersInvicibility && !hitsOnce,
                    fatalHit: shouldKill,
                    damageOnImpactPoint: damageOnImpactPoint,
                    impactPointX: transform.position.x
                    );
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == playerLayer)
                playerDetected = false;
        }
    }
}