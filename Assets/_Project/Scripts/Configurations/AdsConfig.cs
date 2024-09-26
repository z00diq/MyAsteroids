using UnityEngine;

namespace Assets._Project.Scripts.Ads
{
    [CreateAssetMenu(menuName = "Configs/Ads/UnityAds", fileName = "Unity Ads")]
    public class AdsConfig : ScriptableObject
    {
        [SerializeField] private string _androidRewardId;
        [SerializeField] private string _androidInterstitialId;
        [SerializeField] private string _id;

        public string AndroidRewardId => _androidRewardId;
        public string AndroidInterstitialId => _androidInterstitialId;
        public string Id => _id;
    }
}
