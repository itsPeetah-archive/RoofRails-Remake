using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController main;
        public static PlayerController Main => main;

        [Header("Components")]
        [SerializeField] private PlayerPoleController poleController;
        [SerializeField] private Rigidbody rbody;
        [SerializeField] private Collider[] colliders;
        [SerializeField] private Collider playerCollider, poleCollider;
        [SerializeField] private GrindTrigger[] grindTriggers;

        [Header("Stats")]
        [SerializeField] private float invincibilityDuration = 0.6f;
        private bool invincible = false;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float slideDuration = 0.8f;

        [Header("Ground check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayers;

        [Header("Animation")]
        [SerializeField] private Transform graphicsTransform;
        [SerializeField] private Vector3 graphicsSlidingLocalPos;
        [SerializeField] private Quaternion graphicsSlidingLocalRot;

        private Vector3 startingPosition;
        private bool grinding;
        private bool grounded;
        private bool sliding;

        private void Awake()
        {
            if (main != null) Destroy(gameObject);
            else main = this;
        }

        private void Start()
        {
            startingPosition = transform.position;
            GameStateMachine.Main.onGameReset += ResetTransform;
            GameStateMachine.Main.onGameLost += () => SetColliders(false);
        }

        private void FixedUpdate()
        {
            bool g = Physics.CheckSphere(groundCheck.transform.position, 0.05f, groundLayers);
            if (g && !grounded) SetColliders(true);
            grounded = g;
        }

        public void PowerUp()
        {
            poleController.Grow();
        }

        
        public void TakeDamage(bool triggerInvincible = false, bool fatalHit = false, bool damageOnImpactPoint = false, float impactPointX = 0)
        {
            if (invincible)
                return;
            if (fatalHit)
            {
                GameStateMachine.Main.TriggerState(GameState.GameLost);
                return;
            }

            if (triggerInvincible)
                StartCoroutine(InvincibilityCoroutine());

            if (damageOnImpactPoint)
                poleController.CutOnX(impactPointX, invincibilityDuration);
            else
                poleController.Shrink();
        }

        public IEnumerator InvincibilityCoroutine()
        {
            invincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            invincible = false;
        }

        public void Jump() {
            if (grounded && !sliding)
                rbody.velocity = Vector3.up * jumpForce;
        }

        public void Slide() {
            if (grounded && !sliding) {
                StartCoroutine(SlidingCoroutine());
            }
        }

        private IEnumerator SlidingCoroutine() {
            sliding = true;
            graphicsTransform.localPosition = graphicsSlidingLocalPos;
            graphicsTransform.localRotation = graphicsSlidingLocalRot;
            Debug.Log("Started sliding");
            yield return new WaitForSeconds(slideDuration);
            graphicsTransform.localPosition = Vector3.zero;
            graphicsTransform.localRotation = Quaternion.identity;
            sliding = false;
            Debug.Log("Stopped sliding");
        }

        private void ResetTransform() {
            SetColliders(true);
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = Vector3.zero;
            transform.position = startingPosition + Vector3.up * 0.5f;
            transform.rotation = Quaternion.identity;
        }

        private void SetColliders(bool active) {
            playerCollider.enabled = active;
            poleCollider.enabled = active;
        }

        public void StartGrinding() {
            if (!grinding) {
                StartCoroutine(GrindingCoroutine());
            }
        }

        private IEnumerator GrindingCoroutine() {
            grinding = true;
            // Invinc frames
            yield return new WaitForSeconds(0.03f);
            foreach (var t in grindTriggers) grinding &= t.grinding;
            if (!grinding) poleCollider.enabled = false;
        }

        public void StopGrinding()
        {
            if (!grinding) return;
            bool g = false;
            foreach (var t in grindTriggers) g |= t.grinding;
            if (!g) grinding = false;
        }

    }
}