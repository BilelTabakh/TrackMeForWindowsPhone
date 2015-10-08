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
using WinRTXamlToolkit.Controls;
using Windows.UI.Popups;
using Windows.Devices.Sensors;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Graphics.Display;

namespace TrackMe
{
    public sealed partial class StepCounter : Page
    {
        private int numberGoal = 0;
        bool step = false;
        bool goal1 = false;
        bool notification = false;
        double bestSteps = 0;
        bool popupBest = false;

        Accelerometer accelerometer = Accelerometer.GetDefault();
        public StepCounter()
        {
            this.InitializeComponent();
 

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            accelerometer.ReadingChanged += accelerometer_ReadingChanged;
            bestSteps = IsolatedStorageHelper.GetObject<double>("bestSteps");

            MessageDialog msg = new MessageDialog("Put your phone in your pocket and walk to count your steps", "Instruction");
            await msg.ShowAsync();
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
            gg.Value = 0;
        }

        async void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (args.Reading.AccelerationZ < -0.2 && step == false) 
                {
                    step = true;
                    gg.Value++;
                    if (goal1) 
                    {
                        gg.Unit = gg.Value + "/" + numberGoal;
                    }
                    if (gg.Value > bestSteps) 
                    {
                        bestSteps = gg.Value;
                        IsolatedStorageHelper.SaveObject<double>("bestSteps", bestSteps);
                    }
                }
                if (args.Reading.AccelerationZ > 0.2 && step == true)
                {
                    step = false;
                    gg.Value++;
                    if (goal1)
                    {
                        gg.Unit = gg.Value + "/" + numberGoal;
                    }
                    if (gg.Value > bestSteps)
                    {
                        bestSteps = gg.Value;
                        IsolatedStorageHelper.SaveObject<double>("bestSteps", bestSteps);
                        if (!popupBest)
                        {
                            NewScore.Visibility = Visibility.Visible;
                            Storyboard2.Begin();
                            setText.Visibility = Visibility.Collapsed;
                            setText2.Visibility = Visibility.Collapsed;
                            txtGoal.Visibility = Visibility.Collapsed;
                            btnOk.Visibility = Visibility.Collapsed;
                            AudioWinner.Play();
                            popupBest = true;
                        }
                    }
                }



                    if (goal1 && gg.Value > numberGoal && !notification) 
                    {

                    var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

                    var txtNodes = toastXmlContent.GetElementsByTagName("text");
                    txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Congratulation"));
                    txtNodes[1].AppendChild(toastXmlContent.CreateTextNode("Goal achieved"));

                    var toast = new ToastNotification(toastXmlContent);
                    var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                    toastNotifier.Show(toast);
                    notification = true;
                    } 

                
              //  reading.Text = "X: " + args.Reading.AccelerationX.ToString() + "\nY: " + args.Reading.AccelerationY.ToString() + "\nZ: " + args.Reading.AccelerationZ.ToString();
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            bool showmsg = false;
            try
            {
                numberGoal = int.Parse(txtGoal.Text);
                gg.Maximum = numberGoal;
                gg.Unit = gg.Value + "/" + numberGoal;
                goal1 = true;
            }
            catch
            {
                showmsg = true;
            }
            if (showmsg) 
            {
                MessageDialog msg = new MessageDialog("Wrong input", "Error");
                await msg.ShowAsync();
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
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


            dp.Properties.Title = "My Steps By TrackMe";
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

        private async void deleteScore(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Are you sure you want to reset your best Score?", "Warning");
            //Commands
            msg.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(CommandHandlers)));
            msg.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(CommandHandlers)));
            await msg.ShowAsync();
        }
        public void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Yes":
                    IsolatedStorageHelper.SaveObject<double>("bestSteps", 0);
                    gg.Value = 0;
                    Frame.Navigate(typeof(MainPage));
                    break;
                //Quit Button.
                case "Cancel":

                    break;
                //end.
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewScore.Visibility = Visibility.Collapsed;
            setText.Visibility = Visibility.Visible;
            setText2.Visibility = Visibility.Visible;
            txtGoal.Visibility = Visibility.Visible;
            btnOk.Visibility = Visibility.Visible;
        }

        private void goBand(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StepBand));
        }

    }
}
