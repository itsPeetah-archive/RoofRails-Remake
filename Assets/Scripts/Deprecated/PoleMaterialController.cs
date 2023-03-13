using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class PoleMaterialController : MonoBehaviour
    {
        [SerializeField] private float sliderDuration = 0.25f;
        [SerializeField] private string centerCoordPropName = "SET ME!", sideCoordPropName = "SET ME!";

        [SerializeField] private Material material;

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
            while (t <= sliderDuration)
            {
                A = Mathf.Lerp(1, 0, t / sliderDuration);
                material.SetFloat(centerCoordPropName, A);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t <= sliderDuration)
            {
                B = Mathf.Lerp(1, 0, t / sliderDuration);
                material.SetFloat(sideCoordPropName, B);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t <= sliderDuration)
            {
                A = Mathf.Lerp(0, 1, t / sliderDuration);
                material.SetFloat(centerCoordPropName, A);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t <= sliderDuration)
            {
                B = Mathf.Lerp(0, 1, t / sliderDuration);
                material.SetFloat(sideCoordPropName, B);
                t += Time.deltaTime;
                yield return null;
            }


        }
    }
}