using System;
using System.Collections.Generic;
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
    public sealed partial class SeanceTile : UserControl
    {
        public SeanceTile()
        {
            this.InitializeComponent();
            Storyboard anim = (Storyboard)FindName("SeanceTileAnim1");
            anim.Begin();
        }





        private void SeanceTileAnim2_Completed_1(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("SeanceTileAnim1_Inverse");
            anim.Begin();
        }

        private void SeanceTileAnim2_Inverse_Completed_1(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("SeanceTileAnim1");
            anim.Begin();
        }

    }
}
