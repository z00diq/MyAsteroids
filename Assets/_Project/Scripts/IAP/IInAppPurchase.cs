using System;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.IAP
{
    public interface IInAppPurchase
    {
        public Task Initialize();

        public void BuyAdsOff();

        public event Action PurchaseComplite;

    }
}
