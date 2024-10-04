using UnityEngine.Advertisements;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets._Project.Scripts.Ads
{
    public class UnityAds : Ads, IUnityAdsShowListener, IUnityAdsInitializationListener, IUnityAdsLoadListener
    {
        private bool _testMode;
        private bool _isShowEnabled = true;
        TaskCompletionSource<bool> _initTask;
        TaskCompletionSource<bool> _showTask;
       
        public UnityAds(AdsConfig adsConfig) : base(adsConfig)
        {
#if UNITY_EDITOR
            _testMode = true;
#else 
            _testMode = false;
#endif
            _initTask = new TaskCompletionSource<bool>();
            _showTask = new TaskCompletionSource<bool>();
        }

        public override void DisableAds()
        {
            _isShowEnabled = false;
        }

        public override async Task InitializeAds()
        {
            if (_isShowEnabled == false)
                return;

            Advertisement.Initialize(AppId,_testMode,this);

            await _initTask.Task;
        }

        public override void LoadInterstitial()
        {
            Advertisement.Load(InterstitialId,this);
        }

        public override void LoadRewardedAds()
        {
            Advertisement.Load(RewardId, this);
        }

        public async override Task ShowInterstitial()
        {
            if (_isShowEnabled == false)
                return;

            Advertisement.Show(InterstitialId, this);

            await _showTask.Task;
        }

        public override void ShowRewardedAds()
        {
            if (_isShowEnabled == false)
                return;

            Advertisement.Show(RewardId,this);
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) 
        {
            if (placementId.Equals(InterstitialId))
            {
                Advertisement.Load(InterstitialId,this);
                _showTask.SetResult(true);
            }
            else if (placementId.Equals(RewardId))
            {
                Advertisement.Load(RewardId, this);
                GetReward();
            }
        }

        void IUnityAdsInitializationListener.OnInitializationComplete() 
        {
            _initTask.SetResult(true);
        }

        void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message) 
        {
            Debug.Log(error.ToString() + " " + message);
        }

        void IUnityAdsLoadListener.OnUnityAdsAdLoaded(string placementId) { }

        void IUnityAdsLoadListener.OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }

        void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

        void IUnityAdsShowListener. OnUnityAdsShowClick(string placementId) { }

        void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId) { }
    }
}
