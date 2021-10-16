using GoogleMobileAds.Api;
using UnityEngine;

namespace Towerb.Admob
{
  /**
     <summary>広告マネージャ</summary>
  */
  public class AdsManager
    : SingletonBehaviour<AdsManager>
  {
    public bool IsInitialized { private set; get; } = false;

    /**
     */
    void Start()
    {
      MobileAds.Initialize(_ => { this.IsInitialized = true; });
    }
  }
}
