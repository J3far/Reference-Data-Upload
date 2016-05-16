using System;
using System.Windows.Controls;

namespace ReferenceDataUploader.UploadSteps
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Welcome : UserControl,ISwitchable
    {
        public Welcome()
        {
            InitializeComponent();
            navigation_buttons.set_Buttons(false, true, true);
        }

        public void UtilizeState(object state)
        {
            ;
        }

        public object TransferToNextpage()
        {
            return null;
        }

        public void SetPageSwitcher(PageSwitcher pageSwitcher)
        {
            navigation_buttons.parentPageSwitcher = pageSwitcher;
        }
    }
}
