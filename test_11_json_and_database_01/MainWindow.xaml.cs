using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using test_11_json_and_database_01.Database;

namespace test_11_json_and_database_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DoIt();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoIt();
        }

        public void DoIt()
        {
            string jsonFile = File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "json_01.json")).Trim();

            //Console.WriteLine(jsonFile);

            JsonFile_1.RootObject root = JsonConvert.DeserializeObject<JsonFile_1.RootObject>(jsonFile);

            JsonFile_1.Data data = root.Data;

            bool isQuerySuccessfull = DB_test_Queries.InsertDataFromJson_1(data);
            Console.WriteLine("isQuerySuccessfull = " + isQuerySuccessfull);
        }
    }
}
