using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace kitchentimer
{
    public partial class kitchentimerPage : ContentPage
    {
        DateTime finishedAt;
        DateTime stoppedAt;
        IRingTone ringTone;
        DateTime TimeZero = new DateTime(0);
        int Seconds = 0;
        const int MAX_SECONDS = 100 * 60;
        const string START_LABEL = "Start";
        List<Image> Images = new List<Image>();
        int[] TimerImageIndices = new int[] { 0, 1 };
        int[] RingImageIndices = new int[] { 2, 3 };
        
        public kitchentimerPage()
        {

            InitializeComponent();
            ringTone = DependencyService.Get<IRingTone>();
            ResetLabel();
            var children = layout.Children;
            foreach (var child in children)
            {
                if (child.GetType() == typeof(Image)) {
                    Images.Add((Image)child);
                }
            }
            ChangeImage(0);
            foreach (var s in Enum.GetValues(typeof(Sec)))
            {
                var button = this.FindByName<Button>("button" + s.ToString());
                button.Text = TimeLabel((int)s);
                button.Clicked += ((object sender, EventArgs e) => OnSecButtonClick(s));
            }
        }

        private void OnSecButtonClick(Object sec)
        {
            if(finishedAt != TimeZero){
                return;
            }
            Seconds += (int) sec;
            if (Seconds > MAX_SECONDS) {
                Seconds = MAX_SECONDS;
            }
            valueLabel.Text = FormatMinSec(Seconds);
        }

        string TimeLabel(int sec)
        {
            var m = sec / 60;
            var s = sec - m * 60;
            StringBuilder label = new StringBuilder();
            if (m > 0) {
                label.Append(m);
                label.Append("分");
            }
            if (s > 0) {
                label.Append(s);
                label.Append("秒");
            }
            return label.ToString();
        }

        private string FormatMinSec(int sec)
        {
            if (sec < 0) {
                sec = 0;
            }
            var m = sec / 60;
            var s = sec - m * 60;
            return string.Format("{0:00}:{1:00}", m, s);
        }

        private void OnStartButtonClicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            if (Seconds > 0 || stoppedAt > TimeZero) {
                StartTimer();
            } else if (!NextTick()) {
                ResetTimer();
            } else {
                StopTimer();
            }
        }

        private void OnResetButtonClicked(object sender, EventArgs args)
        {
            ResetTimer();
        }

        private async void StartTimer()
        {
            startButton.Text = "Stop";
            if (stoppedAt > TimeZero) {
                finishedAt += DateTime.Now - stoppedAt;
                stoppedAt = TimeZero;
            } else {
                finishedAt = DateTime.Now.AddSeconds(Seconds);
                Seconds = 0;
            }
            while (true)
            {
                if (!Tick()) {
                    break;
                }
                await Task.Delay(100);
            }
        }

        private void StopTimer()
        {
            startButton.Text = "Start";
            stoppedAt = DateTime.Now;
        }

        private void ResetTimer()
        {
            ResetLabel();
            finishedAt = TimeZero;
            ringTone.Stop();
            ChangeImage(0);
        }

        private Boolean Tick()
        {
            if (finishedAt == TimeZero) {
                return false;
            }
            if (stoppedAt > TimeZero) {
                return true;
            }
            if (!NextTick()) {
                TimerFinished();
                return false;
            }
            return true;
        }

        private Boolean isRinging(){
            return finishedAt == TimeZero;
        }

        private Boolean NextTick ()
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = finishedAt - now;
            valueLabel.Text = FormatMinSec((int)diff.TotalSeconds);
            ChangeImage((double)diff.TotalMilliseconds);
            return diff > TimeSpan.Zero;
        }

        private void ChangeImage (double msec)
        {
            int[] indices = new int[] {};
            int t = 0;
            if (isRinging()) {
                indices = RingImageIndices;
                t = (int) msec / 200;
            } else {
                indices = TimerImageIndices;
                t = (int) msec / 1000;
            }
            var i = t % indices.Length;
            var visibleIndex = indices[i];
            for (var n = 0; n < Images.Count; n++) {
                Images[n].IsVisible = (n == visibleIndex);
            }
        }

        private void ResetLabel ()
        {
            valueLabel.Text = "--:--";
            Seconds = 0;
            stoppedAt = TimeZero;
            finishedAt = TimeZero;
            startButton.Text = "Start";
        }

        private void TimerFinished ()
        {
            finishedAt = TimeZero;
            ringTone.Play();
        }
    }
}
