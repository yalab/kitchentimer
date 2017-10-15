using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace kitchentimer
{
    public partial class kitchentimerPage : ContentPage
    {
        DateTime finishTime;
        public kitchentimerPage()
        {
            InitializeComponent();
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            finishTime = DateTime.Now.AddSeconds(10);
            StartTimer();
        }

        async void StartTimer()
        {
            while (true)
            {
                if(!RefreshLabel()){
                    valueLabel.Text = "--:--";
                    break;
                }
                await Task.Delay(60);
            }
        }

        Boolean RefreshLabel()
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = finishTime - now;
            if (diff <= TimeSpan.Zero){
                return false;
            }
            valueLabel.Text = diff.ToString(@"mm\:ss");
            return true;
        }
    }
}
