using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Google.MobileAds;
using UIKit;
using CoreGraphics;
using kitchentimer;
using kitchentimer.iOS;

[assembly: ExportRenderer(typeof(AdMobBanner), typeof(AdMobBannerRenderer))]
namespace kitchentimer.iOS
{
    public class AdMobBannerRenderer : ViewRenderer
    {
        const string adUnitID = "ca-app-pub-3685574646511324/3820005995";

        UIViewController viewCtrl = null;
        BannerView adMobBanner;
        bool viewOnScreen;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (e.OldElement == null)
            {
                foreach (UIWindow v in UIApplication.SharedApplication.Windows)
                {
                    if (v.RootViewController != null)
                    {
                        viewCtrl = v.RootViewController;
                    }
                }
                adMobBanner = new BannerView(AdSizeCons.Banner, new CGPoint(-10, 0))
                {
                    AdUnitID = adUnitID,
                    RootViewController = viewCtrl
                };
                adMobBanner.AdReceived += (sender, args) =>
                {
                    if (!viewOnScreen) AddSubview(adMobBanner);
                    viewOnScreen = true;
                };

                var request = Request.GetDefaultRequest();

                adMobBanner.LoadRequest(request);
                SetNativeControl(adMobBanner);
            }
        }
    }
}