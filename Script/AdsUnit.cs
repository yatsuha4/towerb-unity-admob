using System;
using UnityEngine;

namespace Towerb.Admob
{
  [Serializable]
  public class AdsUnit
  {
    [SerializeField]
    private string android;

    [SerializeField]
    private string ios;

    /**
     */
    public string Id
    {
      get
      {
#if UNITY_ANDROID
        return android;
#elif UNITY_IOS
        return ios;
#else
        return string.Empty;
#endif
      }
    }
  }
}
