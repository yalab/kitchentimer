using System;
using Xamarin.Forms;
using AudioToolbox;

[assembly: Dependency(typeof(kitchentimer.iOS.RingTone))]
namespace kitchentimer.iOS
{
    public class RingTone : IRingTone
    {
        private uint alermSoundId = 1005;

        public void Play()
        {
            var s = new SystemSound(alermSoundId);
            s.PlaySystemSound();
        }

        public void Stop()
        {
        }
    }
}
