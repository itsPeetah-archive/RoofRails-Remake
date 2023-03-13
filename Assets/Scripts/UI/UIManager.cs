using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RoofRails
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUDS")]
        [SerializeField] private GameObject menuHUD;
        [SerializeField] private GameObject gameHUD;
        [SerializeField] private GameObject pauseHUD;
        [SerializeField] private GameObject lostHUD;
        [SerializeField] private GameObject wonHUD;
        private Dictionary<GameState, GameObject> huds = new Dictionary<GameState, GameObject>();

        [Header("Screen fade")]
        [SerializeField] private Image fadeCurtain;
        [SerializeField] private float fadeDuration;

        [Header("Data display")]
        [SerializeField] private TextMeshProUGUI[] gemLabels;
        [SerializeField] private TextMeshProUGUI[] levelLabels;
        [SerializeField] private TextMeshProUGUI winScreenLvlMultLabel;
        [SerializeField] private TextMeshProUGUI winScreenResultLabel;

        private void Start()
        {
            // Dictionary initialization
            huds.Add(GameState.Menu, menuHUD);
            huds.Add(GameState.Playing, gameHUD);
            huds.Add(GameState.Paused, pauseHUD);
            huds.Add(GameState.GameLost, lostHUD);
            huds.Add(GameState.GameWon, wonHUD);

            // Event subscription
            GameStateMachine.Main.onStateChanged += HideHuds;
            GameStateMachine.Main.onGameReset += () => huds[GameState.Menu].SetActive(true);
            GameStateMachine.Main.onGameStarted += () => huds[GameState.Playing].SetActive(true);
            GameStateMachine.Main.onGameUnpause += () => huds[GameState.Playing].SetActive(true);
            GameStateMachine.Main.onGamePause += () => huds[GameState.Paused].SetActive(true);
            GameStateMachine.Main.onGameLost += () => huds[GameState.GameLost].SetActive(true);
            GameStateMachine.Main.onGameWon += ShowWinScreen;

            PlayerDataManager.Main.onGemObtained += UpdateGemDisplays;
            PlayerDataManager.Main.onLevelUp += UpdateLevelDisplays;

            // Game initialization
            HideHuds();
            huds[GameStateMachine.Main.State].SetActive(true);
            fadeCurtain.gameObject.SetActive(false);
            UpdateGemDisplays();
            UpdateLevelDisplays();
        }

        private void HideHuds()
        {
            foreach (var k in huds.Keys) huds[k].SetActive(false);
        }

        // Button callbacks
        public void TogglePause() => GameStateMachine.Main.TogglePause();
        public void StartGame() => GameStateMachine.Main.TriggerState(GameState.Playing);
        public void BackToMenu()
        {
            StopAllCoroutines();
            StartCoroutine(BackToMenu_Coroutine());
        }

        private IEnumerator BackToMenu_Coroutine()
        {
            // Fade out
            SetCurtainAlpha(0);
            fadeCurtain.gameObject.SetActive(true);
            float t = 0;
            while (t <= fadeDuration)
            {
                SetCurtainAlpha(Mathf.Lerp(0, 1, t / fadeDuration));
                t += Time.deltaTime;
                yield return null;
            }

            // Switch hud 
            SetCurtainAlpha(1);
            GameStateMachine.Main.TriggerState(GameState.Menu);

            // Fade back in
            t = fadeDuration;
            while (t > 0)
            {
                SetCurtainAlpha(Mathf.Lerp(0, 1, t / fadeDuration));
                t -= Time.deltaTime;
                yield return null;
            }
            fadeCurtain.gameObject.SetActive(false);
        }

        private void SetCurtainAlpha(float a)
        {
            Color c = fadeCurtain.color;
            c.a = a;
            fadeCurtain.color = c;
        }

        private void UpdateGemDisplays() {
            int l = gemLabels.Length;
            string t = PlayerDataManager.Main.Gems.ToString();
            for (int i = 0; i < l; i++) gemLabels[i].SetText(t);
        }

        private void UpdateLevelDisplays() {
            int l = levelLabels.Length;
            string t = "Level " + PlayerDataManager.Main.Level.ToString();
            for (int i = 0; i < l; i++) levelLabels[i].SetText(t);
        }

        private void ShowWinScreen() {

            winScreenLvlMultLabel.SetText(PlayerDataManager.Main.levelGemMultiplier.ToString());
            winScreenResultLabel.SetText((PlayerDataManager.Main.levelGemMultiplier * PlayerDataManager.Main.Level).ToString());

            huds[GameState.GameWon].SetActive(true);
        }
    }
}
