﻿using Assets._Project.Scripts.Ads;
using Assets._Project.Scripts.IAP;
using Assets.Infrastructure;
using System;
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
        [SerializeField] private Button _rewardAdsButton;
        [SerializeField] private Button _adsDisableButton;
        [SerializeField] private Text _cooridnatesText;
        [SerializeField] private Text _rotationText;
        [SerializeField] private Text _speedText;
        [SerializeField] private Text _laserCountText;
        [SerializeField] private Text _laserReloadTimeText;

        private Ads _ads;
        private IInAppPurchase _inAppPurchase;

        [Inject]
        public void Construct(SceneSecretary sceneSecretary,Ads ads, IInAppPurchase iap)
        {
            _ads = ads;
            _inAppPurchase = iap;
            _restartButton.onClick.AddListener(sceneSecretary.ToGamScene);
            _rewardAdsButton.onClick.AddListener(ads.ShowRewardedAds);
            _adsDisableButton.onClick.AddListener(_inAppPurchase.BuyAdsOff);
            _ads.SendReward += ToGame;
            _inAppPurchase.PurchaseComplite += DisableAdsButton;
            _inAppPurchase.PurchaseComplite += ToGame;
        }

        private void Awake()
        {
            _gameOverPanel.SetActive(false);
            _InfoPanel.SetActive(true);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
            _rewardAdsButton.onClick.RemoveAllListeners();
            _adsDisableButton.onClick.RemoveAllListeners();

            _ads.SendReward -= ToGame;
            _inAppPurchase.PurchaseComplite -= DisableAdsButton;
            _inAppPurchase.PurchaseComplite -= ToGame;

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

        private void ToGame()
        {
            _gameOverPanel.SetActive(false);
            _InfoPanel.SetActive(true);
        }

        private void DisableAdsButton()
        {
            _rewardAdsButton.gameObject.SetActive(false);
            _adsDisableButton.gameObject.SetActive(false);
        }
    }
}
