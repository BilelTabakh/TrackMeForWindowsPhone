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
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using Windows.UI;
using TrackMe;
using Windows.UI.Core;
using Windows.UI.Notifications;
using TrackMe.ViewModel;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Graphics.Display;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrackMe
{

    public sealed partial class SeancePage : Page
    {
        //Chronometre
        DispatcherTimer myTimer = new DispatcherTimer();
        int currentTimeSec = 0;
        int currentTimeMin = 0;
        MainViewModel listSeance;

        Geolocator geolocator = null;
        Geoposition position;
        BasicGeoposition b;
        MapPolyline poly;
        List<BasicGeoposition> positions;
        MapIcon MapIcon;

        bool start = false;
        double distance = 0;
        bool first = true;
        double oldLongitude;
        double oldLatitude;
        double speed = 0;
        double calories = 0;
        double poids;
        bool popup = false;
        //List<TrackMe.Seance> listSeance;

        
        public SeancePage()
        {
            this.InitializeComponent();
           
            myTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            myTimer.Tick += myTimer_Tick;
            btnStop.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            listSeance = new MainViewModel();
             MapIcon = new MapIcon();

             MapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
             MapIcon.Title = "You are Here";
             MapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/ic_runMap.png"));
             
             

            poly = new MapPolyline();
            poly.StrokeColor = Colors.Blue;
            poly.StrokeThickness = 3;
            positions = new List<BasicGeoposition>();
            //listSeance = new List<Seance>();

            /*var myPosition1 = new Windows.Devices.Geolocation.BasicGeoposition();
            myPosition1.Latitude = 41.7446;
            myPosition1.Longitude = -087.7915;

            var myPosition2 = new Windows.Devices.Geolocation.BasicGeoposition();
            myPosition2.Latitude = 4.7446;
            myPosition2.Longitude = -08.7915;

            var myPosition3 = new Windows.Devices.Geolocation.BasicGeoposition();
            myPosition3.Latitude = 42.7446;
            myPosition3.Longitude = -18.7915;
            /*    positions.Add(new BasicGeoposition()
                {
                    Latitude = myPosition1.Latitude,
                    Longitude = myPosition2.Longitude
                });
            positions.Add(myPosition1);
            positions.Add(myPosition2);
            positions.Add(myPosition3);
            poly.Path = new Geopath(positions);
            MyMap.MapElements.Add(poly);*/
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

        void myTimer_Tick(object sender, object e)
        {
            currentTimeSec++;
            txtChronoSec.Text = currentTimeSec.ToString();
            if (currentTimeSec >= 60)
            {
                currentTimeSec = 0;
                currentTimeMin++;
                txtChronoMin.Text = currentTimeMin.ToString();
                txtChronoSec.Text = currentTimeSec.ToString();
               
            }

        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string connectionProfileInfo = string.Empty;
                try
                {


                    ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                    if (InternetConnectionProfile == null)
                    {
                        MessageDialog msg = new MessageDialog("Your phone seems to be disconnected", "Unavailable internet");
                        msg.Commands.Add(new UICommand("Wi-Fi Settings", new UICommandInvokedHandler(CommandHandlers)));
                        msg.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(CommandHandlers)));
                    }
                }
                catch
                {
                }

                poids = IsolatedStorageHelper.GetObject<double>("poids");
                if (poids == 0)
                {
                    MessageDialog msg1 = new MessageDialog("Please check your info first!", "Ckeck Info");
                    msg1.Commands.Add(new UICommand("My infos", new UICommandInvokedHandler(CommandHandlers)));
                    await msg1.ShowAsync();
                }

                geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                geolocator.DesiredAccuracyInMeters = 1;
                geolocator.MovementThreshold = 1; // The units are meters
                geolocator.PositionChanged += geolocator_PositionChanged;



                // txttest.Text = MyMap.Center.Position.Longitude + "\n" + MyMap.Center.Position.Longitude;
                // txttest.Text = args.Position.Coordinate.Point.Position.Latitude + "\n" + args.Position.Coordinate.Point.Position.Longitude;

                //ENABLE THE LOCATION COMPATIBILITY
                position = await geolocator.GetGeopositionAsync();

                await MyMap.TrySetViewAsync(position.Coordinate.Point, 18D);

                MySlider.Value = MyMap.ZoomLevel;
                DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            }
            catch 
            {
                popup = true;
            }
            if (popup)
            {
                MessageDialog msg = new MessageDialog("Your GPS seems to be desactivated, please activate it", "Error");
                await msg.ShowAsync();
            }
        }
        public async void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Wi-Fi Settings":
                    Uri uri1 = new Uri("ms-settings-wifi:");
                await Windows.System.Launcher.LaunchUriAsync(uri1);
                    break;
                //Quit Button.
                case "Cancel":
                    break;
                case "My infos":
                    Frame.Navigate(typeof(InfosPage));
                    break;
                //end.
            }
        }
        async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
               // txttest.Text = args.Position.Coordinate.Point.Position.Longitude.ToString() + "\n" + args.Position.Coordinate.Point.Position.Latitude.ToString();

                b.Latitude = args.Position.Coordinate.Point.Position.Latitude;
                b.Longitude = args.Position.Coordinate.Point.Position.Longitude;
                MyMap.Center = new Geopoint(b);
                MapIcon.Location = new Geopoint(b);
                MyMap.MapElements.Add(MapIcon);

                if (start)
                {
                    if (first)
                    {
                        oldLongitude = args.Position.Coordinate.Point.Position.Longitude;
                        oldLatitude = args.Position.Coordinate.Point.Position.Latitude;
                        first = false;
                    }
                    positions.Add(b);
                    poly.Path = new Geopath(positions);
                    MyMap.MapElements.Add(poly);
                    distance += calculateDistance(oldLongitude, oldLatitude, b.Longitude, b.Latitude);

                    oldLongitude = b.Longitude;
                    oldLatitude = b.Latitude;
                    if (currentTimeMin != 0)
                    {
                        speed = (distance / (currentTimeMin * 60)) * 3.1;
                        txtSpeed.Text = speed + "";
                    }
                    else
                    {
                        speed = (distance / currentTimeSec) * 3.1;
                        txtSpeed.Text = speed + "";
                    }

                    /*
                         Activité faible (moins de 30 minutes d'activité par jour) : 2 100 Kcalories.
                         Activité modérée (30 minutes d'activité chaque jour) : 2500 à 2 700 Kcalories.
                         Activité forte (plus de 1 heure d'activité par jour) : 3 000 à 3 500 Kcalories.
                     */
                    //Nombre de kcal dépensées = votre Poids (kg) x Distance (km)
                    calories = poids*(distance / 1000);
                    calories =(int)calories;

                    txtCalories.Text = calories + "";
                    txtDistance.Text = distance + "";

                    
                    
                }
            });
        }
        private void MySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MyMap != null)
            {
                MyMap.ZoomLevel = e.NewValue;
            }

        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            start = true;
            myTimer.Start();

            btnStart.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnStop.Visibility = Windows.UI.Xaml.Visibility.Visible;

        }

       
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            myTimer.Stop();


            if (txtSpeed.Text == "Wait") 
            {
                speed = 0;
            }
            distance = (double)((int)(distance * 100)) / 100;
            speed = (double)((int)(speed * 100)) / 100;

            Seance s = new Seance(DateTime.Now, speed, calories, distance, currentTimeMin);

            //All Seances
            if (IsolatedStorageHelper.GetObject<List<Seance>>("allSeances") == null)
            {
                listSeance.Seances.Add(s);
                IsolatedStorageHelper.SaveObject<List<Seance>>("allSeances",listSeance.Seances);
            }
            else
            {
                listSeance.Seances = IsolatedStorageHelper.GetObject<List<Seance>>("allSeances");
                listSeance.Seances.Add(s);
                IsolatedStorageHelper.SaveObject<List<Seance>>("allSeances", listSeance.Seances);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            
            start = false;
            btnShare.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Collapsed;
            
            //Frame.Navigate(typeof(MainPage));
        }


        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Uri uri1 = new Uri("zune:navigate?appid=[App Id]");
            await Windows.System.Launcher.LaunchUriAsync(uri1);
        }

        private double calculateDistance(double fromLong, double fromLat,
                double toLong, double toLat)
        {
            double d2r = Math.PI / 180;
            double dLong = (toLong - fromLong) * d2r;
            double dLat = (toLat - fromLat) * d2r;
            double a = Math.Pow(Math.Sin(dLat / 2.0), 2) + Math.Cos(fromLat * d2r)
                    * Math.Cos(toLat * d2r) * Math.Pow(Math.Sin(dLong / 2.0), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = 6367000 * c;
            return Math.Round(d);
        }

        private async void goShare(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(RenderedGrid);
            DataTransferManager.ShowShareUI();
        }

        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            var photoFile = await getPhotoForShare(); //function that will get the screenshot as a storage file


            dp.Properties.Title = "My Jogging By TrackMe";
            dp.Properties.Description = "Sharing the Screenshot";
            dp.SetStorageItems(new List<StorageFile> { photoFile });
            //dp.SetWebLink(new Uri("http://seattletimes.com/ABPub/2006/01/10/2002732410.jpg"));
            deferral.Complete();

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        // Handle DataRequested event and provide DataPackage to share the screenshot

        async Task<StorageFile> getPhotoForShare()
        {

            //Create sample file; replace if exists.
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.CreateFileAsync("Steps.jpg", CreationCollisionOption.ReplaceExisting);

            Guid encoderId = BitmapEncoder.JpegEncoderId; //make image .jpeg format

            try
            {
                using (var stream = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    await CaptureToStreamAsync(RenderedGrid, stream, encoderId);
            }

            catch
            {


            }
            return sampleFile;
        }
        //Creates RenderTargetBitmap from UI Element
        async Task<RenderTargetBitmap> CaptureToStreamAsync(FrameworkElement uielement, IRandomAccessStream stream, Guid encoderId)
        {
            try
            {
                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(uielement);
                var pixels = await renderTargetBitmap.GetPixelsAsync();
                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                var encoder = await BitmapEncoder.CreateAsync(encoderId, stream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    logicalDpi,
                    logicalDpi,
                    pixels.ToArray());

                await encoder.FlushAsync();
                return renderTargetBitmap;

            }
            catch
            {

            }

            return null;
        }
     





        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt.Text = String.Format("{0}, {1}",
                MyMap.Center.Position.Latitude,
                MyMap.Center.Position.Longitude
                );
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var myPosition = new Windows.Devices.Geolocation.BasicGeoposition();
            myPosition.Latitude = 41.7446;
            myPosition.Longitude = -087.7915;

            var myPoint = new Windows.Devices.Geolocation.Geopoint(myPosition);
            if (await MyMap.TrySetViewAsync(myPoint, 10D))
            {
            }

        }
*/
    }
}

