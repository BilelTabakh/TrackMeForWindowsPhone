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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;

namespace TrackMe
{

    public sealed partial class InfosPage : Page
    {
        double taille;
        double poids;
        double iMC;
        bool showMessage = false;
        MessageDialog msg;
        public InfosPage()
        {
            this.InitializeComponent();

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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (IsolatedStorageHelper.GetObject<double>("taille") == 0 || IsolatedStorageHelper.GetObject<double>("poids") == 0 || IsolatedStorageHelper.GetObject<double>("IMC") == 0)
            {
                txtTaille.Text = "0";
                txtPoids.Text = "0";
                txtIMC.Text = "no Data";
            }
            else {
                double poids = IsolatedStorageHelper.GetObject<double>("poids");
                double taille = IsolatedStorageHelper.GetObject<double>("taille");
                double IMCt = IsolatedStorageHelper.GetObject<double>("IMC");
                txtPoids.Text =poids+ "";
                txtTaille.Text = taille + "";
                txtIMC.Text ="BMI= "+IMCt;

                if (IMCt <= 15)
                {
                    txtInterpretation.Text = "Famine";
                }
                if (IMCt <= 18.5 && IMCt > 15) 
                {
                    txtInterpretation.Text = "Underweight";
                }
                else if (IMCt <= 25 && IMCt > 18.5)
                {
                    txtInterpretation.Text = "Ideal Weight";
                }
                else if (IMCt <= 30 && IMCt > 25)
                {
                    txtInterpretation.Text = "Overweight";
                }
                else if (IMCt <= 35 && IMCt > 30)
                {
                    txtInterpretation.Text = " Moderate\n Obesity";
                }
                else if (IMCt <= 40 && IMCt > 35)
                {
                    txtInterpretation.Text = "Severe\n Obesity";
                }
                else
                    txtInterpretation.Text = "Morbid\n Obesity";
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }



        private async void Hello(object sender, RoutedEventArgs e)
        {
            msg = new MessageDialog("Wrong Input", "Warning!");
            try
            {
                double taille1 = double.Parse(txtTaille.Text);
                taille = taille1;
                poids = double.Parse(txtPoids.Text);
                //iMC = poids / ((taille / 100) * (taille / 100));
                taille = taille / 100;
                var t2 = taille * taille;
                iMC = poids / t2;
                iMC = (double)((int)(iMC * 100)) / 100;

                if (iMC < 5 || iMC > 100) 
                {
                    await msg.ShowAsync();
                    return;
                    
                }

                txtIMC.Text = "BMI= "+iMC;

                IsolatedStorageHelper.SaveObject("taille", taille1);
                IsolatedStorageHelper.SaveObject("poids", poids);
                IsolatedStorageHelper.SaveObject("IMC", iMC);

                if (iMC <= 15)
                {
                    txtInterpretation.Text = "Famine";
                }
                if (iMC <= 18.5 && iMC > 15)
                {
                    txtInterpretation.Text = "Underweight";
                }
                else if (iMC <= 25 && iMC > 18.5)
                {
                    txtInterpretation.Text = "Ideal\n Weight";
                }
                else if (iMC <= 30 && iMC > 25)
                {
                    txtInterpretation.Text = "Overweight";
                }
                else if (iMC <= 35 && iMC > 30)
                {
                    txtInterpretation.Text = " Moderate\n Obesity";
                }
                else if (iMC <= 40 && iMC > 35)
                {
                    txtInterpretation.Text = "Severe\n Obesity";
                }
                else if(iMC > 40)
                    txtInterpretation.Text = "Morbid\n Obesity";
                
            }
            catch
            {        
                showMessage = true;

                /*
                var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

                var txtNodes = toastXmlContent.GetElementsByTagName("text");
                txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Erreur"));
                txtNodes[1].AppendChild(toastXmlContent.CreateTextNode("Saisie non conforme"));

                var toast = new ToastNotification(toastXmlContent);
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.Show(toast);
                


                /*ToastTemplateType toastType = ToastTemplateType.ToastText02;

                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastType);

                XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
                toastTextElement[0].AppendChild(toastXml.CreateTextNode("Hello C# Corner"));
                toastTextElement[1].AppendChild(toastXml.CreateTextNode("I am poping you from a Winmdows Phone App"));

                IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                ((XmlElement)toastNode).SetAttribute("duration", "long");

                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast); */
            }
            if (showMessage)
            {
                await msg.ShowAsync();
                showMessage = false;
            }
            //ScreenShotShare s = new ScreenShotShare(ok);
            

        }
    }
}
