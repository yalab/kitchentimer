using Xamarin.Forms;
using System;

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
            finishTime = DateTime.Now.AddMinutes(1);
            valueLabel.Text = finishTime.ToString("mm:ss");
        }
    }
}
