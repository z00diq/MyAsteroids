using Assets.Infrastructure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts
{
    public class ShipUI: MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _InfoPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _cooridnatesText;
        [SerializeField] private Text _rotationText;
        [SerializeField] private Text _speedText;
        [SerializeField] private Text _laserCountText;
        [SerializeField] private Text _laserReloadTimeText;

        [Inject]
        public void Construct(SceneSecretary sceneSecretary)
        {
            _restartButton.onClick.AddListener(sceneSecretary.ReloadMainSceneAsSingle);
        }

        private void Awake()
        {
            _gameOverPanel.SetActive(false);
            _InfoPanel.SetActive(true);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
        }

        public void OnPositionChanged(Vector3 position)
        {
            string text = $"X:{position.x} Y:{position.y}";
            _cooridnatesText.text=text;
        }

        public void OnRotationChanged(Vector3 eulerAngels)
        {
            _rotationText.text = eulerAngels.z.ToString();
        }

        public void OnSpeedChanged(float speed)
        {
            _speedText.text = speed.ToString();
        }

        public void OnLaserCountChanged(int count)
        {
            _laserCountText.text = count.ToString();
        }

        public void OnLaserReloadTimeChanged(float time)
        {
            _laserReloadTimeText.text = time.ToString();
        }

        public void OnGameOver()
        {
            _gameOverPanel.SetActive(true);
            _InfoPanel.SetActive(false);
        }
    }
}
