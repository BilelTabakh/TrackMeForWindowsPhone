using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrackMe
{
    public sealed partial class CalendarPage : Page
    {
        List<Calendrier> s;
        public CalendarPage()
        {
            this.InitializeComponent();

            s = new List<Calendrier>();

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageHelper.GetObject<List<Calendrier>>("Dates") != null)
            {
                s = IsolatedStorageHelper.GetObject<List<Calendrier>>("Dates");
            }
            lst.ItemsSource = s;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtcommentaire.Text.Count() > 100)
            {
                MessageDialog msg = new MessageDialog("Only a comment with a maximum of 100 characters is allowed", "Wrong input");
                await msg.ShowAsync();
            }
            else
            {
                Calendrier d = new Calendrier();
                d.Date = cal.Date.DayOfWeek + " " + cal.Date.Month + " " + cal.Date.Year;
                d.Commentaire = txtcommentaire.Text;
                d.Temps = txttime.Time + "";
                s.Add(d);
                IsolatedStorageHelper.SaveObject<List<Calendrier>>("Dates", s);
                Frame.Navigate(typeof(CalendarPage));
            }
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst.SelectedItems.Count == 1)
            {
                btndetails.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
            }
            if (lst.SelectedItems.Count > 1)
            {
                btnDelete.Visibility = Visibility.Visible;
                btndetails.Visibility = Visibility.Collapsed;
            }
            if (lst.SelectedItems.Count == 0)
            {
                btnDelete.Visibility = Visibility.Collapsed;
                btndetails.Visibility = Visibility.Collapsed;
            }
        }

        private async void deleteClicked(object sender, RoutedEventArgs e)
        {
            if (lst.SelectedItem != null)
            {
                MessageDialog msg = new MessageDialog("Are you sure you want to delete the record?", "Delete?");

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
                    foreach (Calendrier cal1 in items)
                    {
                        s.Remove(cal1);
                    }
                    IsolatedStorageHelper.SaveObject<List<Calendrier>>("Dates", s);
                    Frame.Navigate(typeof(CalendarPage));
                    break;
                //Quit Button.
                case "Cancel":

                    break;
                //end.
            }
        }

        private void detailClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DetailCalendrier), lst.SelectedItem);
        }
    }
}
