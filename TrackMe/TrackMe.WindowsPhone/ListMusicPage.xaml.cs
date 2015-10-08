using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrackMe
{

    public sealed partial class ListMusicPage : Page
    {
        List<StorageFile> f;
        List<String> ames;
        string a;
        public ListMusicPage()
        {
            this.InitializeComponent();
            f = new List<StorageFile>();
            ames = new List<string>();
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            f = await this.GetFilesInMusic();
            foreach (StorageFile s in f)
            {
                a = s.DisplayName;
                ames.Add(a);
            }
            MessageDialog msg = new MessageDialog(f.Count + " nbr Images");
            await msg.ShowAsync();
            lst.ItemsSource = f;
        }
        // first - a method to retrieve files from folder recursively 
        private async Task RetriveFilesInFolder(List<StorageFile> list, StorageFolder parent)
        {
            foreach (var item in await parent.GetFilesAsync()) list.Add(item);
            foreach (var item in await parent.GetFoldersAsync()) await RetriveFilesInFolder(list, item);
        }

        private async Task<List<StorageFile>> GetFilesInMusic()
        {
            StorageFolder folder = KnownFolders.PicturesLibrary;
            List<StorageFile> listOfFiles = new List<StorageFile>();
            await RetriveFilesInFolder(listOfFiles, folder);
            return listOfFiles;
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            Frame.Navigate(typeof(MusicPage), lst.SelectedItem);
        }

    }
}
