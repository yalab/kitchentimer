using System;
using Xamarin.Forms;
using Android.Media;
using Android.Net;
using Android.Content;
using Android.App;

[assembly: Dependency(typeof(kitchentimer.Droid.RingTone))]
namespace kitchentimer.Droid
{
    public class RingTone : IRingTone
    {
        private Ringtone ringTone;
        public RingTone()
        {
            Android.Net.Uri alert = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);

            if (alert == null)
            {
                alert = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                if (alert == null)
                {
                    alert = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
                }
            }
            Context context = Android.App.Application.Context;
            ringTone = RingtoneManager.GetRingtone(context, alert);
        }

        public void Play()
        {
            ringTone.Play();
        }

        public void Stop()
        {
            ringTone.Stop();
        }
    }
}
