using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;


namespace TrackMe
{
    public class ScreenShotShare
    {
        public Windows.UI.Xaml.UIElement s;
        //Creates RenderTargetBitmap from UI Element
        
        public ScreenShotShare(Windows.UI.Xaml.UIElement s)
        {
            this.s = s;
            takeScreenShot();
        }

        public async void takeScreenShot() 
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(s);
            DataTransferManager.ShowShareUI();
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }
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
                    await CaptureToStreamAsync((Windows.UI.Xaml.FrameworkElement)s, stream, encoderId);
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
                    /*pixels.ToArray()*/ null);

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
