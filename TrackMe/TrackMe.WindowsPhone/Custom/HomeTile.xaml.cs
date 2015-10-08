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
    public sealed partial class HomeTile : UserControl
    {
        int faceSelected = 1;
        public HomeTile()
        {
            this.InitializeComponent();
            CheckForAnimation();
        }

        private void HomeTileAnimTop1_Part1_Completed(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("HomeTileAnimTop1_Part2");
            anim.Begin();
        }

        private void HomeTileAnimTop2_Part1_Completed(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("HomeTileAnimTop2_Part2");
            anim.Begin();
        }

        private void HomeTileAnimTop1_Part2_Completed(object sender, object e)
        {
            CheckForAnimation();
        }

        private void HomeTileAnimTop2_Part2_Completed(object sender, object e)
        {
            CheckForAnimation();
        }

        private void CheckForAnimation()
        {
            if (faceSelected == 1)
            {
                faceSelected = 2;
                Storyboard anim = (Storyboard)FindName("HomeTileAnimTop1_Part1");
                anim.Begin();
            }
            else if (faceSelected == 2)
            {
                faceSelected = 1;
                Storyboard anim = (Storyboard)FindName("HomeTileAnimTop2_Part1");
                anim.Begin();
            }
        }
    }
}
