using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Litlab.Services.Base;
using Litlab.Game;
using Litlab.Game.Managers;

namespace RadicalGraphics.Services.Game
{
    public abstract class GameStoreService : Service<GameStoreService>
    {

        private static GameStoreService m_instance = GeneratePlatformDependantInstance();
        public static GameStoreService Instance { get => m_instance; }

        public abstract void RefreshStore(StoreRefreshRequest request, Action<StoreRefreshResponse> OnResult);
        public abstract void LockStore(StoreLockRequest request,Action<StoreLockResponse> OnResult);
        public abstract void UpgradeTotem(UpgradeTotemRequest request, Action<UpgradeTotemCallback> OnResult);
        public abstract void PurchaseTitan(StorePurchaseRequest request, Action<StorePurchaseResponse> OnResult);
        public abstract void GetStore(GetStoreRequest request, Action<GetStoreResponse> OnResult);
        public abstract void PurchaseExperience(PurchaseExperienceRequest request, Action<PurchaseExperienceResponse> OnResult);
        private static GameStoreService GeneratePlatformDependantInstance()
        {

            if (Application.isMobilePlatform)
                return new GameStoreServiceMobile();

            else return new GameStoreServiceMobile();

        }

        #region Requests
        [System.Serializable]
        public class PurchaseExperienceRequest : GameRequestBase
        {
        }
        [System.Serializable]
        public class GetStoreRequest : GameRequestBase
        {
        }
        [System.Serializable]
        public class StoreRefreshRequest : GameRequestBase
        {
            
        }
        [System.Serializable]
        public class StoreSellRequest : GameRequestBase
        {
            public int titanId;
        }
        [System.Serializable]
        public class StoreLockRequest : GameRequestBase
        {
            public bool locked;
        }
        [System.Serializable]
        public class UpgradeTotemRequest : GameRequestBase
        {

        }
        [System.Serializable]
        public class StorePurchaseRequest : GameRequestBase
        {
            public long titanId;
        }
        #endregion

        #region Responses
        [System.Serializable]
        public class PurchaseExperienceResponse : GameResponseBase
        {
            public float experience;

            public int purchaseCost;

        }
        [System.Serializable]
        public class GetStoreResponse : GameResponseBase
        {
            public List<Titan> store = new List<Titan>();

            public List<Probability> probabilities = new List<Probability>();

        }
        [System.Serializable]
        public class StoreRefreshResponse : GameResponseBase
        {
            public List<Titan> store = new List<Titan>();

            public List<Probability> probabilities = new List<Probability>();
        }
        [System.Serializable]
        public class StoreLockResponse : GameResponseBase
        {
        }
        [System.Serializable]
        public class UpgradeTotemCallback : GameResponseBase
        {

        }
        [System.Serializable]
        public class StorePurchaseResponse : ResponseBase
        {
            public GameData response;
        }
        [System.Serializable]
        public class StoreSellResponse : GameResponseBase
        {
        }
        #endregion



    }



}