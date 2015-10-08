using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace TrackMe
{

    public sealed partial class StatsPage : Page
    {
        List<Seance> listSeance;
        public StatsPage()
        {
            this.InitializeComponent();
            
            this.Loaded += MainPage_Loaded;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }
        private void LoadChartContents()
        {
            listSeance = IsolatedStorageHelper.GetObject<List<Seance>>("allSeances");
            //(PieChart.Series[0] as PieSeries).ItemsSource = financialStuffList;
            //(ColumnChart.Series[0] as ColumnSeries).ItemsSource = financialStuffList;
            (LineChart.Series[0] as LineSeries).ItemsSource = listSeance;
            //(BarChart.Series[0] as BarSeries).ItemsSource = financialStuffList;
            //(BubbleChart.Series[0] as BubbleSeries).ItemsSource = financialStuffList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
              Seance s = new Seance();
            s = (Seance)e.Parameter;
            txtDate.Text = s.DateSeance+"";
            txtCalories.Text = s.Calories+"";
            txtDistance.Text = s.Distance+"";
            txtDuree.Text = s.Duree+"";
            txtVitesse.Text = s.Vitesse+"";
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
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

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(RenderedGrid);
            DataTransferManager.ShowShareUI();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        // Handle DataRequested event and provide DataPackage to share the screenshot
        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            var photoFile = await getPhotoForShare(); //function that will get the screenshot as a storage file


            dp.Properties.Title = "My Jogging Evolution By TrackMe";
            dp.Properties.Description = "Sharing the Screenshot";
            dp.SetStorageItems(new List<StorageFile> { photoFile });
            //dp.SetWebLink(new Uri("http://seattletimes.com/ABPub/2006/01/10/2002732410.jpg"));
            deferral.Complete();

        }
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
     
    }
}
