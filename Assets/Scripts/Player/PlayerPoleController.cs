using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class PlayerPoleController : MonoBehaviour
    {
        [Header("Scale")]
        [SerializeField] private Transform poleTransform;
        [SerializeField] private float growRatio = 0.2f;
        [SerializeField] private float shrinkRatio = 0.1f;

        private float startingScaleX;
        private float growIncrement;
        private float shrinkDecrement;
        private Vector3 scale;

        [Header("Relative cut options")]
        [SerializeField] private Transform leftmostPoint;
        [SerializeField] private Transform centerPoint;
        [SerializeField] private Transform rightmostPoint;
        [SerializeField] private float slidingAnimationDuration = 0.2f;

        [Header("Material")]
        [SerializeField] private float materialSliderDuration = 0.2f;
        [SerializeField] private string centerCoordPropName = "SET ME!", sideCoordPropName = "SET ME!";
        [SerializeField] private Material material;

        private void Start()
        {
            scale = poleTransform.localScale;
            startingScaleX = scale.x;

            growIncrement = growRatio * startingScaleX;
            shrinkDecrement = shrinkRatio * startingScaleX;

            material.SetFloat(centerCoordPropName, 1);
            material.SetFloat(sideCoordPropName, 1);

            GameStateMachine.Main.onGameReset += () => {
                scale.x = startingScaleX;
                poleTransform.localScale = scale;
            };
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

        public void Grow()
        {
            DoFlashEffect();
            scale.x += growIncrement;
            poleTransform.localScale = scale;
        }

        public void Shrink()
        {
            scale.x = Mathf.Max(0.01f, scale.x - shrinkDecrement);
            poleTransform.localScale = scale;

            // TODO Change lose condition
            if (scale.x <= 0.01f)
            {
                Debug.Log("Game over: you lose!");
                GameStateMachine.Main.TriggerState(GameState.GameLost);
            }
        }

        public void CutOnX(float impactXCoord, float slidingDelay = 0) {
            /*
             *  c: center
             *  i: impact
             *  l: leftmost
             *  r: righmost
             *  
             *  [=============x===]
             *  l        c    i   r
             *  
             *  d = abs(l - r)
             *  p = i > c ? abs(l - i) : abs(r - i)
             *  scale = p / d
             *  posx = (l + i) / 2
             */

            StopAllCoroutines();
            material.SetFloat(centerCoordPropName, 1);
            material.SetFloat(sideCoordPropName, 1);

            float c, i, l, r, d, p, x;
            float li, ri;
            bool right;
            // A lot of useless assignment but I wanted to keep this close to the graph/formulas
            c = centerPoint.position.x;
            l = leftmostPoint.position.x;
            r = rightmostPoint.position.x;
            i = impactXCoord; 
            d = Mathf.Abs(l - r);
            right = i >= c;
            li = Mathf.Abs(l - i);
            ri = Mathf.Abs(r - i);
            p = right ? li : ri;
            x = c + (right ? -ri : li) * 0.7f;

            scale.x = scale.x * p / d;
            poleTransform.localScale = scale;

            //poleTransform.localPosition = Vector3.right * x;
            StartCoroutine(SlideToCenter(x, slidingDelay));
        }

        public void DoFlashEffect()
        {
            StopCoroutine("FlashEffectCoroutine");
            StartCoroutine("FlashEffectCoroutine");
        }

        private IEnumerator FlashEffectCoroutine()
        {
            float A = 1, B = 1;

            material.SetFloat(centerCoordPropName, A);
            material.SetFloat(sideCoordPropName, B);

            float t = 0;
            while (t <= materialSliderDuration)
            {
                A = Mathf.Lerp(1, 0, t / materialSliderDuration);
                material.SetFloat(centerCoordPropName, A);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t <= materialSliderDuration)
            {
                B = Mathf.Lerp(1, 0, t / materialSliderDuration);
                material.SetFloat(sideCoordPropName, B);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t <= materialSliderDuration)
            {
                A = Mathf.Lerp(0, 1, t / materialSliderDuration);
                material.SetFloat(centerCoordPropName, A);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t <= materialSliderDuration)
            {
                B = Mathf.Lerp(0, 1, t / materialSliderDuration);
                material.SetFloat(sideCoordPropName, B);
                t += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator SlideToCenter(float startingXCoord, float initialDelay) {


            Vector3 localPos = new Vector3(startingXCoord, 0, 0);
            float t = 0;

            poleTransform.localPosition = localPos;

            yield return new WaitForSeconds(initialDelay);
            while (t <= slidingAnimationDuration)
            {
                localPos.x = Mathf.Lerp(startingXCoord, 0, t / slidingAnimationDuration);
                poleTransform.localPosition = localPos;

                t += Time.deltaTime;
                yield return null;
            }

            poleTransform.localPosition = Vector3.zero;

        }
    }
}