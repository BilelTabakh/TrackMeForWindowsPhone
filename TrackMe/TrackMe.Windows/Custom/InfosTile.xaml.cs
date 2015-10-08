using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TrackMe.Custom
{
    public sealed partial class InfosTile : UserControl
    {
        public InfosTile()
        {
            this.InitializeComponent();
            Storyboard anim = (Storyboard)FindName("InfosTileAnim1");
            anim.Begin();
        }

        private void InfosTileAnim2_Completed_1(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("InfosTileAnim1_Inverse");
            anim.Begin();
        }

        private void InfosTileAnim2_Inverse_Completed_1(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("InfosTileAnim1");
            anim.Begin();
        }
    }
}
