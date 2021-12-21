namespace ClipboardMachinery.Core.TagKind.Impl.DataTypes {

    public class EnumReference {

        #region Properties

        internal int ReferenceKey { get; }

        public string CachedValue { get; set; }

        #endregion

        public EnumReference(int referenceKey) {
            ReferenceKey = referenceKey;
        }

    }

}
