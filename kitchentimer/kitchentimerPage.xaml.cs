using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace kitchentimer
{
    public partial class kitchentimerPage : ContentPage
    {
        DateTime finishTime;
        IRingTone ringTone;
        DateTime TimeZero = new DateTime(0);
        int Seconds = 0;

        public kitchentimerPage()
        {
            InitializeComponent();
            ringTone = DependencyService.Get<IRingTone>();
            finishTime = TimeZero;

            foreach (var s in Enum.GetValues(typeof(Sec)))
            {
                var button = this.FindByName<Button>("button" + s.ToString());
                button.Text = ((int)s / 60).ToString() + "分";
                button.Clicked += ((object sender, EventArgs e) => OnButtonClick(s));
            }
        }

        void OnButtonClick(Object sec)
        {
            Seconds += (int) sec;
            var m = Seconds / 60;
            var s = Seconds - m * 60;
            valueLabel.Text = string.Format("{0:00}:{1:00}", m, s);
        }

        private void OnStartButtonClicked(object sender, EventArgs args)
        {
            StartTimer();
        }

        private void OnResetButtonClicked(object sender, EventArgs args)
        {
            ResetTimer();
        }

        private async void StartTimer()
        {
            if(Seconds < 1)
            {
                return;
            }
            finishTime = DateTime.Now.AddSeconds(Seconds);
            while (true)
            {
                if(!Tick()){
                    break;
                }
                await Task.Delay(100);
            }
        }

        private void ResetTimer()
        {
            ResetLabel();
            finishTime = TimeZero;
            ringTone.Stop();
        }

        private Boolean Tick()
        {
            if(finishTime == TimeZero){
                return false;
            }
            DateTime now = DateTime.Now;
            TimeSpan diff = finishTime - now;
            if (diff <= TimeSpan.Zero){
                TimerFinished();
                return false;
            }
            valueLabel.Text = diff.ToString(@"mm\:ss");
            return true;
        }

        private void ResetLabel ()
        {
            valueLabel.Text = "--:--";
            Seconds = 0;
        }

        private void TimerFinished ()
        {
            ringTone.Play();
        }
    }
}
