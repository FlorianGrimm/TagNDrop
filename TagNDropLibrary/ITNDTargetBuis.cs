namespace TagNDropLibrary {
    public interface ITNDTargetBuis {
        ITNDTargetBuis Init(TNDTargetModel targetModel, ITNDDataAccess dataAccess, ITNDApplicationBuis applicationBuis);
    }
}