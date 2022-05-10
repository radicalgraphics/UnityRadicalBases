using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RadicalGraphics.Services.Game
{
    public class GameStoreServiceMobile : GameStoreService
    {

        private const string m_getStoreUrl = "https://localhost:7135/api/Game/store";
        private const string m_lockStoreUrl = "https://localhost:7135/api/Game/store";
        private const string m_purchaseTitanUrl = "https://localhost:7135/api/Game/store";
        private const string m_refreshStoreUrl = "https://localhost:7135/api/Game/store";
        private const string m_upgradeTotemUrl = "https://localhost:7135/api/Game/store";
        private const string m_purchaseExperienceUrl = "https://localhost:7135/api/Game/store";

        public override void GetStore(GetStoreRequest request, Action<GetStoreResponse> OnResult)
        {
            Send(m_getStoreUrl,request,OnResult);
        }

        public override void LockStore(StoreLockRequest request, Action<StoreLockResponse> OnResult)
        {
            Send(m_lockStoreUrl, request, OnResult);
        }

        public override void PurchaseExperience(PurchaseExperienceRequest request, Action<PurchaseExperienceResponse> OnResult)
        {
            Send(m_purchaseExperienceUrl, request, OnResult);
        }

        public override void PurchaseTitan(StorePurchaseRequest request, Action<StorePurchaseResponse> OnResult)
        {
            Send(m_purchaseTitanUrl, request, OnResult);
        }

        public override void RefreshStore(StoreRefreshRequest request, Action<StoreRefreshResponse> OnResult)
        {
            Send(m_refreshStoreUrl, request, OnResult);
        }

        public override void UpgradeTotem(UpgradeTotemRequest request, Action<UpgradeTotemCallback> OnResult)
        {
            Send(m_upgradeTotemUrl, request, OnResult);
        }
    }
}
