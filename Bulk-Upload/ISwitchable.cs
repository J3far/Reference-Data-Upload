using System.Collections.Generic;

namespace ReferenceDataUploader
{
    public interface ISwitchable
    {
        void UtilizeState(Dictionary<string, object> objects);
        Dictionary<string, object> TransferToNextpage();
        void SetPageSwitcher(PageSwitcher pageSwitcher);
    }
}