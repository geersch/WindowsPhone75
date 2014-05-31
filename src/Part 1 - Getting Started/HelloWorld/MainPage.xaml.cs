using System.Windows;
using Microsoft.Phone.Controls;

namespace HelloWorld
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MyTextBlock.Text = "Hello, World!";
        }
    }
}