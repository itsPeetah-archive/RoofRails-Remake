using System;
using UnityEngine;

namespace RoofRails
{
    public class PlayerDataManager : MonoBehaviour
    {
        private static PlayerDataManager main;
        public static PlayerDataManager Main => main;

        [SerializeField] private string levelProgressPrefKey = "CurrentLevel";
        [SerializeField] private string ownedGemsKey = "OwnedGems";

        private int currentLevel, ownedGems;
        public int Level => currentLevel;
        public int Gems => ownedGems;

        private bool hasLeveledUp = false;
        public int levelGemMultiplier = 1;

        public event Action onGemObtained;
        public event Action onLevelUp;

        private void Awake()
        {
            if (main != null) Destroy(gameObject);
            else
            {
                main = this;

                currentLevel = PlayerPrefs.GetInt(levelProgressPrefKey, 1);
                ownedGems = PlayerPrefs.GetInt(ownedGemsKey, 0);
            }
        }

        private void Start()
        {
            GameStateMachine.Main.onGameLost += Save;
            GameStateMachine.Main.onGameWon += () => { Save(); hasLeveledUp = true; } ;
            GameStateMachine.Main.onGameReset += LevelUp;
        }

        public void Save() {
            PlayerPrefs.SetInt(levelProgressPrefKey, currentLevel);
            PlayerPrefs.SetInt(ownedGemsKey, ownedGems);
        }

        public void LevelUp() {

            if (!hasLeveledUp) return;
            hasLeveledUp = false;
            GetGems(Level * levelGemMultiplier);
            currentLevel += 1;
            Save();
            onLevelUp?.Invoke();
        }

        public void GetGems(int amount = 1)
        {
            ownedGems += amount;
            onGemObtained?.Invoke();
        }
    }
}