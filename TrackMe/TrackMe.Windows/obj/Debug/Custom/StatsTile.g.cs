﻿

#pragma checksum "C:\Users\Bilel\Desktop\TrackMe\TrackMe\TrackMe.Windows\Custom\StatsTile.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3F3E7CCBBAED9CA17C4FA1B7C3120CAB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrackMe.Custom
{
    partial class StatsTile : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 14 "..\..\Custom\StatsTile.xaml"
                ((global::Windows.UI.Xaml.Media.Animation.Timeline)(target)).Completed += this.StatsTileAnimTop1_Part1_Completed;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 23 "..\..\Custom\StatsTile.xaml"
                ((global::Windows.UI.Xaml.Media.Animation.Timeline)(target)).Completed += this.StatsTileAnimTop1_Part2_Completed;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 33 "..\..\Custom\StatsTile.xaml"
                ((global::Windows.UI.Xaml.Media.Animation.Timeline)(target)).Completed += this.StatsTileAnimTop2_Part1_Completed;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 42 "..\..\Custom\StatsTile.xaml"
                ((global::Windows.UI.Xaml.Media.Animation.Timeline)(target)).Completed += this.StatsTileAnimTop2_Part2_Completed;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


