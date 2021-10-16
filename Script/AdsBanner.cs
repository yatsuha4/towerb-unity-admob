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
    private AdsUnit unit;

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
        while(!AdsManager.Instance.IsInitialized)
        {
          yield return null;
        }
        var size = 
          AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(this.unit.Id, size, this.position);
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
#if !UNITY_EDITOR
        if(GetComponentInParent<Canvas>() is Canvas canvas &&
           GetComponent<LayoutElement>() is LayoutElement layoutElement)
        {
          var height = 
            Mathf.CeilToInt(bannerView.GetHeightInPixels() / canvas.scaleFactor);
          Debug.Log($"AdsBanner height = {height}");
          if(height > layoutElement.preferredHeight)
          {
            layoutElement.preferredHeight = height;
          }
        }
#endif
      }
    }

    /**
     */
    private void OnAdLoaded(object sender, EventArgs args)
    {
      Debug.Log("AdsBanner.OnAdLoaded");
      state = States.Loaded;
    }

    /**
     */
    private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
      Debug.Log("AdsBanner.OnAdFailedToLoad");
      bannerView = null;
      state = States.Failed;
    }
  }
}
