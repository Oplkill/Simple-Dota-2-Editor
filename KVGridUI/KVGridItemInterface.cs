using System.Collections.Generic;
using System.Drawing;

namespace KVGridUI
{
    public interface KVGridItemInterface
    {
        int ItemHeight { get; }

        int ItemWidth { get; set; }

        ItemTypes ItemType { get; }

        KVGridBlock ParentBlock { get; set; }

        KVGrid GridOwner { get; set; }

        string KeyText { get; set; }

        /// <summary>
        /// Can be null if doesnt contain value (eq Block)
        /// Will be empty if value empty
        /// </summary>
        string ValueText { get; set; }

        int UpdateHeight();

        bool Selected { get; set; }
    }
}