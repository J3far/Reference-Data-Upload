namespace ReferenceDataUploader
{
    public interface ISwitchable
    {
        void UtilizeState(object state);
        object TransferToNextpage();
        void SetPageSwitcher(PageSwitcher pageSwitcher);
    }
}