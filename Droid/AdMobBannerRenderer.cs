using kitchentimer;
using kitchentimer.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobBanner), typeof(AdMobBannerRenderer))]
namespace kitchentimer.Droid
{
    public class AdMobBannerRenderer : ViewRenderer<AdMobBanner, Android.Gms.Ads.AdView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdMobBanner> e)
        {
            const string adUnitID = "ca-app-pub-3685574646511324/3820005995";

            base.OnElementChanged(e);

            if (Control == null)
            {
                var adMobBanner = new Android.Gms.Ads.AdView(Forms.Context);
                adMobBanner.AdSize = Android.Gms.Ads.AdSize.Banner;
                adMobBanner.AdUnitId = adUnitID;

                var requestbuilder = new Android.Gms.Ads.AdRequest.Builder();
                adMobBanner.LoadAd(requestbuilder.Build());

                SetNativeControl(adMobBanner);
            }
        }
    }
}