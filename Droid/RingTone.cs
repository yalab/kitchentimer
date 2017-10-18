using System;
using Xamarin.Forms;
using Android.Media;
using Android.Net;
using Android.Content;
using Android.App;
using Android.OS;

[assembly: Dependency(typeof(kitchentimer.Droid.RingTone))]
namespace kitchentimer.Droid
{
    public class RingTone : IRingTone
    {
        private Ringtone ringTone;
        private Context context;
        private Vibrator vibrator;
        private AudioManager audiomanager;
        public RingTone()
        {
            Android.Net.Uri alert = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
            context = Android.App.Application.Context;
            ringTone = RingtoneManager.GetRingtone(context, alert);
            vibrator = (Vibrator)context.GetSystemService(Context.VibratorService);
            audiomanager = (AudioManager)context.GetSystemService(Context.AudioService);
        }

        public void Play()
        {
            var volume = audiomanager.GetStreamVolume(Stream.Ring);
            if (volume > 0) {
                ringTone.Play();
            } else {
                vibrator.Vibrate(1000 * 3);
            }
        }

        public void Stop()
        {
            ringTone.Stop();
            vibrator.Cancel();
        }
    }
}
