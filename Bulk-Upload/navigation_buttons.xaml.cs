using ReferenceDataUploader.UploadSteps;
using System.Windows.Controls;

namespace ReferenceDataUploader
{
    /// <summary>
    /// Interaction logic for switcher_pageControl.xaml
    /// </summary>
    public partial class Navigation_Buttons : UserControl
    {
        public PageSwitcher parentPageSwitcher { get; set; }

        public Navigation_Buttons()
        {
            InitializeComponent();
        }

        public void set_Buttons(bool hasPreviousPage, bool hasNextPage, bool hasFinish)
        {

            button_previous.IsEnabled = hasPreviousPage;
            button_next.IsEnabled = hasNextPage;
            button_finish.IsEnabled = hasFinish;
        }

        private void next_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (parentPageSwitcher != null) parentPageSwitcher.Navigate(+1);
        }

        private void finish_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (parentPageSwitcher != null) parentPageSwitcher.Close();
        }

        private void previous_clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if(parentPageSwitcher != null) parentPageSwitcher.Navigate(-1);
        }
    }
}
