using System;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.Ads
{
    public abstract class Ads
    {
        protected string RewardId;
        protected string InterstitialId;
        protected string AppId;

        public event Action SendReward;

        public Ads(AdsConfig adsConfig)
        {
            RewardId = adsConfig.AndroidRewardId;
            InterstitialId = adsConfig.AndroidInterstitialId;
            AppId = adsConfig.Id;
        }

        public abstract Task InitializeAds();

        public abstract void LoadRewardedAds();
        
        public abstract void LoadInterstitial();

        public abstract void ShowRewardedAds();

        public abstract Task ShowInterstitial();

        protected void GetReward()
        {
            SendReward?.Invoke();
        }
    }
}
