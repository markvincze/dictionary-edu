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
using DictionaryMeta;

namespace DictionaryVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var dict = new Dictionary<int, string> { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" } };

            var extractor = new DictionaryMetadataExtractor<int, string>();

            var meta = extractor.ExtractMetadata(dict);
        }
    }
}
