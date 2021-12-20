using System;

namespace ClipboardMachinery.Core.TagKind.Impl.Schemas {

    public class UriTagKindSchema : TagKindSchema<Uri> {

        #region Properties

        public override string Name { get; } = "Uniform Resource Identifier";

        public override string Description { get; } = "A value representing an URL address, SSH connection or file system path.";

        public override string Icon { get; } = "IconCompass";

        #endregion

        #region Logic

        public override bool TryRead(string value, out Uri result) {
            if (value.Length > 3 && value.Substring(1, 1) == ":") {
                value = "file:///" + value;
            }

            value = value.Replace('\\', '/');

            if (Uri.IsWellFormedUriString(value, UriKind.Absolute)) {
                result = new Uri(value, UriKind.Absolute);
                return true;
            }

            result = null;
            return false;
        }

        public override bool TryWrite(Uri value, out string result) {
            result = value.AbsoluteUri;
            return true;
        }

        public override string GetText(Uri value) {
            string displayValue = value.AbsoluteUri;

            if (displayValue.StartsWith("file:///")) {
                displayValue = displayValue.Substring(8);
            }

            return displayValue;
        }

        #endregion

    }

}
