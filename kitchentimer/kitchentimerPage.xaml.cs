using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Text;

namespace kitchentimer
{
    public partial class kitchentimerPage : ContentPage
    {
        DateTime finishTime;
        DateTime stoppedTime;
        IRingTone ringTone;
        DateTime TimeZero = new DateTime(0);
        int Seconds = 0;
        const int MAX_SECONDS = 100 * 60;
        const string START_LABEL = "Start";

        public kitchentimerPage()
        {
            InitializeComponent();
            ringTone = DependencyService.Get<IRingTone>();
            ResetLabel();
            foreach (var s in Enum.GetValues(typeof(Sec)))
            {
                var button = this.FindByName<Button>("button" + s.ToString());
                button.Text = TimeLabel((int)s);
                button.Clicked += ((object sender, EventArgs e) => OnSecButtonClick(s));
            }
        }

        private void OnSecButtonClick(Object sec)
        {
            Seconds += (int) sec;
            if (Seconds > MAX_SECONDS){
                Seconds = MAX_SECONDS;
            }
            valueLabel.Text = FormatMinSec(Seconds);
        }

        string TimeLabel(int sec)
        {
            var m = sec / 60;
            var s = sec - m * 60;
            StringBuilder label = new StringBuilder();
            if (m > 0)
            {
                label.Append(m);
                label.Append("分");
            }
            if (s > 0)
            {
                label.Append(s);
                label.Append("秒");
            }
            return label.ToString();
        }

        private string FormatMinSec(int sec)
        {
            var m = sec / 60;
            var s = sec - m * 60;
            return string.Format("{0:00}:{1:00}", m, s);
        }

        private void OnStartButtonClicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            if (Seconds > 0 || stoppedTime > TimeZero) {
                StartTimer();
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
            if (stoppedTime > TimeZero) {
                finishTime += DateTime.Now - stoppedTime;
                stoppedTime = TimeZero;
            } else {
                finishTime = DateTime.Now.AddSeconds(Seconds);
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
            stoppedTime = DateTime.Now;
        }

        private void ResetTimer()
        {
            ResetLabel();
            finishTime = TimeZero;
            ringTone.Stop();
        }

        private Boolean Tick()
        {
            if (finishTime == TimeZero) {
                return false;
            }
            if (stoppedTime > TimeZero) {
                return true;
            }
            DateTime now = DateTime.Now;
            TimeSpan diff = finishTime - now;
            if (diff <= TimeSpan.Zero) {
                TimerFinished();
                return false;
            }
            valueLabel.Text = FormatMinSec((int)diff.TotalSeconds);
            return true;
        }

        private void ResetLabel ()
        {
            valueLabel.Text = "--:--";
            Seconds = 0;
            stoppedTime = TimeZero;
            finishTime = TimeZero;
            startButton.Text = "Start";
        }

        private void TimerFinished ()
        {
            finishTime = TimeZero;
            ringTone.Play();
        }
    }
}
