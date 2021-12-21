using System.Threading.Tasks;

namespace ClipboardMachinery.Core.TagKind.Impl.Schemas {

    public class TextTagKindSchema : TagKindSchema<string> {

        #region Properties

        public override string Name { get; } = "Text";

        public override string Description { get; } = "A value stored as a plain text.";

        public override string Icon { get; } = "IconText";

        #endregion

        #region Logic

        public override bool TryRead(string tagType, string value, out string result) {
            result = value;
            return true;
        }

        public override bool TryWrite(string value, out string result) {
            result = value;
            return true;
        }

        public override Task<string> GetText(string value) {
            return Task.FromResult(value);
        }

        #endregion

    }

}
