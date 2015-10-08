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
using Windows.UI.Xaml.Navigation;
using TrackMe.ViewModel;
using Windows.UI.Popups;
using System.Threading.Tasks;
namespace TrackMe
{
    public sealed partial class ListSeance : Page
    {
        //List<Seance> listSeance;
        MainViewModel listSeance;
        int indiceSeance ;
        MessageDialog msg;
        public ListSeance()
        {
            this.InitializeComponent();

            listSeance = new MainViewModel();
            listSeance.Seances = IsolatedStorageHelper.GetObject<List<Seance>>("allSeances");

            indiceSeance = IsolatedStorageHelper.GetObject<int>("indiceSeance");

            lst.ItemsSource = listSeance.Seances;
            lst.SelectionChanged += lst_SelectionChanged;


            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }

        }



        void  lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var items = lst.SelectedItems;
            if (items.Count == 1)
            {
                stat.Visibility = Visibility.Visible;
            }
            else 
            {
                stat.Visibility = Visibility.Collapsed;  
            }

            if (items.Count == 0)
            {
                appBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                appBar.Visibility = Visibility.Visible;
            }


            if(lst.SelectedItem != null)
            del.Visibility = Visibility.Visible;
            else
            del.Visibility = Visibility.Collapsed;
            //var items = lst.SelectedItems;
            //Frame.Navigate(typeof(StatsPage), lst.SelectedItem);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          /*  Seance s = new Seance();
            s = (Seance)e.Parameter;
            txt1.Text = "Id: " + s.Id;
            */
            del.Visibility = Visibility.Collapsed;
            stat.Visibility = Visibility.Collapsed;
            appBar.Visibility = Visibility.Collapsed;
        }
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        { 
            if (lst.SelectedItem != null) {
            msg = new MessageDialog("Are you sure you want to delete the record?", "Delete?");

            //Commands
            msg.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(CommandHandlers)));
            msg.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(CommandHandlers)));
            await msg.ShowAsync();
            }
        }
        public void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Yes":
                    var items = lst.SelectedItems;
                    foreach (Seance s in items)
                    {
                        listSeance.Seances.Remove(s);
                    }
                    IsolatedStorageHelper.SaveObject<List<Seance>>("allSeances", listSeance.Seances);
                    Frame.Navigate(typeof(ListSeance));
                    break;
                //Quit Button.
                case "Cancel":

                    break;
                //end.
            }
        }
        private  void AppBarButton_ClickBack(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
            
        }
        private void AppBarButton_ClickStat(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StatsPage), lst.SelectedItem);

        }

    }
}
