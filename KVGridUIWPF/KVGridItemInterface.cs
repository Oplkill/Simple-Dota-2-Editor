namespace KVGridUIWPF
{
    public interface KVGridItemInterface
    {
        int ItemHeight { get; }

        ItemTypes ItemType { get; }

        KVGridBlock ParentBlock { get; set; }

        KVGrid GridOwner { get; set; }

        string KeyText { get; set; }

        /// <summary>
        /// Can be null if doesnt contain value (eq Block)
        /// Will be empty if value empty
        /// </summary>
        string ValueText { get; set; }

        bool Selected { get; set; }

        int Id { get; set; }

        KVGrid.TextChangedFunc OnTextChanged { get; set; }
    }
}