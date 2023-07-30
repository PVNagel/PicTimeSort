using Microsoft.Win32;
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
using System.Windows.Forms;

namespace PicTimeSortApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var FileExplorerDialog = new FolderBrowserDialog())
            {
                FileExplorerDialog.ShowDialog();
                SelectedFolderTextBox.Text = FileExplorerDialog.SelectedPath;
            }
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
