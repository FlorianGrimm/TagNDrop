namespace TagNDropLibrary {
    public interface ITNDDropBuis {
        ITNDDropBuis Init(TNDDropModel dropModel);

        void ApplyConfiguration(ITNDApplicationBuis applicationBuis);

        void DoDragEnter(TNDFavorite favorite, System.Windows.IDataObject data);

        void DoDrop(TNDFavorite favorite, System.Windows.IDataObject data);

        void DoGiveFeedback(TNDFavorite favorite, System.Windows.GiveFeedbackEventArgs e);

        void DoPaste(TNDFavorite favorite, System.Windows.IDataObject data);

        void UpdateFormatDescription();

        string GetFormatDescription(System.Windows.IDataObject data);

        void HandleDrops(TNDFavorite favorite, TNDDropItem[] drops);

        void Activating();

        void Deactivating();
    }
}
