using System.Globalization;

namespace ClipboardMachinery.Core.TagKind.Impl.Schemas {

    public class NumericTagKindSchema : TagKindSchema<decimal> {

        #region Properties

        public override string Name { get; } = "Numeric";

        public override string Description { get; } = "A value stored as number allowing for decimal places if desired.";

        public override string Icon { get; } = "IconNumeric";

        #endregion

        #region Logic

        public override bool TryRead(string value, out decimal result) {
            bool isSuccessfullyParsed = decimal.TryParse(
                s: value,
                style: NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingWhite,
                provider: CultureInfo.InvariantCulture,
                result: out decimal numericValue
            );

            result = numericValue;
            return isSuccessfullyParsed;
        }

        public override bool TryWrite(decimal value, out string result) {
            result = value.ToString(CultureInfo.InvariantCulture);
            return true;
        }

        public override string GetText(decimal value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

    }

}
