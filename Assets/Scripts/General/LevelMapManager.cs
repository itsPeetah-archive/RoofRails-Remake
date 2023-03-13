using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoofRails
{
    public class LevelMapManager : MonoBehaviour
    {
        [SerializeField] private Transform[] levels;

        private void Start()
        {

            PlayerDataManager.Main.onLevelUp += () => LoadLevel(PlayerDataManager.Main.Level);

            LoadLevel(PlayerDataManager.Main.Level);
        }

        private void LoadLevel(int lvl) {
            int idx = (lvl - 1) % levels.Length;
            Debug.Log($"Loading level {lvl} ({idx})");

            for (int i = 0; i < levels.Length; i++) {
                levels[i].gameObject.SetActive(i == idx);
            }
        }
    }
}