using ReferenceDataUploader.UploadSteps;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ReferenceDataUploader
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PageSwitcher: Window
    {
        private UserControl [] pages;
        private int currentPageNumber = 0;
        private UserControl currentPageControl;

        public PageSwitcher()
        {
            InitializeComponent();
            pages = new UserControl[3];
            pages[0] = new Welcome();
            pages[1] = new DataSource();
            pages[2] = new DataDestination();


            ISwitchable temp;
            for (int i = 0; i < pages.Length; i++)
            {
                temp = pages[i] as ISwitchable;
                if (temp != null) temp.SetPageSwitcher(this);
            }


            Navigate(currentPageNumber);
        }
        
        public void Navigate(int page)
        {
            if (currentPageNumber+page >= pages.Length || currentPageNumber + page < 0) return;

            currentPageNumber = currentPageNumber + page;
            currentPageControl = pages[currentPageNumber];

            //if going backwords, then dont pass an objects
            if (page > 0) {

                ISwitchable current = currentPageControl as ISwitchable;
                ISwitchable previous = pages[currentPageNumber-1] as ISwitchable;

                if (current != null && previous != null)
                    current.UtilizeState(previous.TransferToNextpage()) ;
            }

            this.Content = currentPageControl;

        }
    }
}
