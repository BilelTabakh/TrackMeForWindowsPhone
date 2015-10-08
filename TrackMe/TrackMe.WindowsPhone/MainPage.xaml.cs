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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
namespace TrackMe
{
    public sealed partial class MainPage : Page
    {
        MainViewModel lastSeance;
        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            lastSeance = new MainViewModel();
            lastSeance.Seances = IsolatedStorageHelper.GetObject<List<Seance>>("allSeances");

            if (lastSeance.Seances == null || lastSeance.Seances.Count == 0)
            {
                txtVitesse.Text = "No Data";
                txtDuree.Text = "No Data";
                txtDistance.Text = "No Data";
                txtCalories.Text = "No Data";
                txtDate.Text = "No Data";
            }
            else
            {

                Seance lastOne = lastSeance.Seances.Last();
                txtCalories.Text = lastOne.Calories + " Kcal";
                txtDistance.Text = lastOne.Distance + " Km";
                txtDuree.Text = lastOne.Duree + " min";
                txtVitesse.Text = lastOne.Vitesse + " Km/h";
                txtDate.Text = lastOne.DateSeance.ToString();
            }
            double bestStep = IsolatedStorageHelper.GetObject<double>("bestSteps");
            txtBestStep.Text = bestStep + "";

            double bestPush = IsolatedStorageHelper.GetObject<double>("bestPush");
            txtBestPush.Text = bestPush + "";
        }

        private void goSeance(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SeancePage));
        }

        private void goAbout(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void goListSeance(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ListSeance));
        }

        private void goInfo(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InfosPage));
        }

        private async void goPlanning(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(ListMusicPage));
           /* string uriToLaunch = @"http://www.bing.com";
            var uri = new Uri(uriToLaunch);
            */
            Uri uri1 = new Uri("zune:navigate?appid=[App Id]");
            await Windows.System.Launcher.LaunchUriAsync(uri1);


            /*
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
           
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            Int16 dueTimeInHours = 3;
            DateTime dueTime = DateTime.Now.AddSeconds(dueTimeInHours);
            
            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, dueTime);
            scheduledToast.Id = "Future_Toast";
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
             * */
        }

        private void goStepCounter(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StepCounter));
        }

        private void goPushUps(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PushUpsPage));
        }

        private void goCalendar(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CalendarPage));
        }

    }
}
