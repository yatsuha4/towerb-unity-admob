using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Towerb.Admob
{
  /**
     <summary>バナー広告</summary>
  */
  public class AdsBanner
    : MonoBehaviour
  {
    private enum States {
      Loading, 
      Loaded, 
      Failed
    }

    [SerializeField]
    private AdPosition position;

    [SerializeField]
    private bool show = true;

    [SerializeField]
    private string unitId_android;

    [SerializeField]
    private string unitId_ios;

    private string UnitId
    {
      get
      {
#if UNITY_ANDROID
        return this.unitId_android;
#elif UNITY_IOS
        return this.unitId_ios;
#else
        return "";
#endif
      }
    }

    private static BannerView bannerView;
    private static States state;

    /**
     */
    void Awake()
    {
      if(!this.show)
      {
        if(bannerView != null)
        {
          bannerView.Destroy();
          bannerView = null;
        }
        Destroy(this.gameObject);
      }
    }

    /**
     */
    IEnumerator Start()
    {
      if(bannerView == null)
      {
        var size = 
          AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(this.UnitId, size, this.position);
        bannerView.OnAdLoaded += this.OnAdLoaded;
        bannerView.OnAdFailedToLoad += this.OnAdFailedToLoad;
        state = States.Loading;
        var request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        while(state == States.Loading)
        {
          yield return null;
        }
      }
      if(state == States.Loaded)
      {
        var canvas = GetComponentInParent<Canvas>();
        var height = 
          Mathf.CeilToInt(bannerView.GetHeightInPixels() / canvas.scaleFactor);
        Debug.Log($"AdsBanner height = {height}");
        var layoutElement = GetComponent<LayoutElement>();
        if(height > layoutElement.preferredHeight)
        {
          layoutElement.preferredHeight = height;
        }
      }
    }

    /**
     */
    private void OnAdLoaded(object sender, EventArgs args)
    {
      state = States.Loaded;
    }

    /**
     */
    private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
      bannerView = null;
      state = States.Failed;
    }
  }
}
