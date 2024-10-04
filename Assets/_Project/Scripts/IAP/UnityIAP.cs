using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Assets._Project.Scripts.IAP
{
    public class UnityIAP : IInAppPurchase, IDetailedStoreListener
    {
        private const string ADS_DISABLE_ID = "ads.off";
        private TaskCompletionSource<bool> _initTask;
        private IStoreController _storeController;

        public UnityIAP()
        {
            _initTask = new TaskCompletionSource<bool>();
        }

        public event Action PurchaseComplite;

        void IInAppPurchase.BuyAdsOff()
        {
            _storeController.InitiatePurchase(ADS_DISABLE_ID);
        }

        async Task IInAppPurchase.Initialize()
        {
            InitializationOptions options = new InitializationOptions();

            await UnityServices.InitializeAsync(options);

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(ADS_DISABLE_ID, ProductType.NonConsumable);
        
            UnityPurchasing.Initialize(this, builder);

            await _initTask.Task;
        }

        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _initTask.SetResult(true);
            _storeController = controller;
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("IAP initialization failed");
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log("IAP initialization failed");
        }

        void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log("IAP cannot complite purchase");
        }

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            throw new NotImplementedException();
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Product purchased = purchaseEvent.purchasedProduct;
            
            if(purchased.definition.id == ADS_DISABLE_ID)
            {
                Debug.Log("Disabling ads...");
                PurchaseComplite?.Invoke();
            }

            return PurchaseProcessingResult.Complete;
        }
    }
}
