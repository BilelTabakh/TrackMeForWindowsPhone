using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrackMe
{

    public sealed partial class CustomSplashScreen : Page
    {
        public CustomSplashScreen(SplashScreen sc, bool loadState)
        {
            this.InitializeComponent();
            Storyboard1.Begin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
