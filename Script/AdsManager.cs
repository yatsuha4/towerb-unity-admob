using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine;

namespace Towerb.Admob
{
    /**
       <summary>広告マネージャ</summary>
    */
    public class AdsManager
        : SingletonBehaviour<AdsManager>
    {
        [SerializeField]
        private AdsBanner banner;

        public bool IsInitialized { private set; get; } = false;

        /**
         */
        void Awake()
        {
            base.Awake();
            if(this.banner is null || !this.banner.gameObject.activeSelf)
            {
                AdsBanner.DestroyBanner();
            }
        }

        /**
         */
        void Start()
        {
            MobileAds.Initialize(_ => { this.IsInitialized = true; });
        }
    }
}
