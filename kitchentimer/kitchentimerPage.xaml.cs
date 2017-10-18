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

        public kitchentimerPage()
        {
            InitializeComponent();
            ringTone = DependencyService.Get<IRingTone>();
        }

        private void OnStartButtonClicked(object sender, EventArgs args)
        {
            StartTimer();
        }

        private void OnStopButtonClicked(object sender, EventArgs args)
        {
            StopTimer();
        }

        private async void StartTimer()
        {
            finishTime = DateTime.Now.AddSeconds(10);
            while (true)
            {
                if(!Tick()){
                    break;
                }
                await Task.Delay(60);
            }
        }

        private void StopTimer()
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
        }

        private void TimerFinished ()
        {
            ringTone.Play();
        }
    }
}
