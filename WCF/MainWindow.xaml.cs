using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;      //ezt kézzel írtuk be.
using WCF.MNB;

namespace WCF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //MNBArfolyamServiceSoapClient client = new MNBArfolyamServiceSoapClient();
            //client.
        }
        MNBArfolyamServiceSoapClient client = new MNBArfolyamServiceSoapClient();

        private void Button_Click(object sender, RoutedEventArgs e) //Mai
        {
            string answer = client.GetCurrentExchangeRates();
            XDocument xml = XDocument.Parse(answer);        //xml-ezés
            //Rate (új osztály, az árfolyam adatokról)

            DataGrid.ItemsSource = ProcessCurrentExchangeRates(xml);        //a Grid auto kinyeri hogy milyen oszlopain lesznek.
            //a mai dátumot lekérdeztük
            //De az is kell, hogy adott idő közöttit kapjunk vissza.
        }

        private List<Rate> ProcessCurrentExchangeRates(XDocument xml)
        {
            XElement dayElement = xml.Root.Element("Day"); //<-- kis-nagybetű érzékeny
            //<Day date="2004-07-16"> ezt a formátumot a DateTime remekül felismeri
            DateTime date = DateTime.Parse(dayElement.Attribute("date").Value);
            //<Rate curr="AUD" unit="1">146,3</Rate> ebből tudjuk a new Rate()-t    146,3 a Value
            IEnumerable<Rate> rates = dayElement.Elements("Rate").Select(rateElement =>
                new Rate(rateElement.Attribute("curr").Value,
                    date, double.Parse(rateElement.Value),
                    double.Parse(rateElement.Attribute("unit").Value)));       //n elemű listából n elemű listát visszaad.

            return rates.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Kell a getInfo
            string answer = client.GetInfo();
            XDocument xml = XDocument.Parse(answer);
            SetUp(xml);
        }
        private void SetUp(XDocument xml)
        {
            //MNBExhangeRatesQueryValues>
            //<FirstDate>1949-01-03</FirstDate>
            //<LastDate>2004-07-16</LastDate>
            //
            //vagy Parse-oljuk
            DateTime firstDate = DateTime.Parse(xml.Root.Element("FirstDate").Value);
            //vagy kasztoljuk
            DateTime lastDate = (DateTime)xml.Root.Element("LastDate");

            List<string> currencies = xml.Root.Element("Currencies").Elements("Curr").Select(currElement => currElement.Value).ToList();

            datePickerStart.DisplayDateStart =    //ez a legelső dátum ami kiválasztható belőle
                datePickerEnd.DisplayDateStart = firstDate;

            datePickerStart.DisplayDateEnd = datePickerEnd.DisplayDateEnd = lastDate;
            listBoxCurrencies.ItemsSource = currencies;
        }
    }
}
