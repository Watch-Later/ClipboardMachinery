using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClipboardMachinery.Core.DataStorage;
using ClipboardMachinery.Core.DataStorage.Schema;
using ClipboardMachinery.Core.TagKind.Impl.DataTypes;
using ServiceStack.OrmLite;

namespace ClipboardMachinery.Core.TagKind.Impl.Schemas {

    public class EnumTagKindSchema : TagKindSchema<EnumReference> {

        #region Properties

        public override string Name { get; } = "Enumeration";

        public override string Description => "A value from predefined list of options.";

        public override string Icon { get; } = "IconList";

        #endregion

        private readonly IDatabaseAdapter databaseAdapter;

        public EnumTagKindSchema(IDatabaseAdapter databaseAdapter) {
            this.databaseAdapter = databaseAdapter;
        }

        #region Logic

        public override bool TryRead(string tagType, string value, out EnumReference result) {
            result = null;

            if (!int.TryParse(value, out int key)) {
                return false;
            }
            
            result = new EnumReference(key);
            return true;
        }

        public override bool TryWrite(EnumReference value, out string result) {
            result = value?.ReferenceKey.ToString();
            return value != null;
        }

        public override async Task<string> GetText(EnumReference value) {
            if (value.CachedValue != null) {
                return value.CachedValue;
            }

            EnumType enumType = await databaseAdapter.Connection.SingleAsync<EnumType>(
                record => record.Id == value.ReferenceKey
            );

            value.CachedValue = enumType?.Value;
            return value.CachedValue;
        }

        #endregion

    }

}
