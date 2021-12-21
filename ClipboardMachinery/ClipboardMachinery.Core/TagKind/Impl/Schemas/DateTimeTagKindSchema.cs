using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ClipboardMachinery.Core.TagKind.Impl.Schemas {

    public class DateTimeTagKindSchema : TagKindSchema<DateTime> {

        #region Properties

        public override string Name { get; } = "Date and time";

        public override string Description => $"A value with date and time components in '{targetCulture.DateTimeFormat.ShortDatePattern} {targetCulture.DateTimeFormat.ShortTimePattern}' format.";

        public override string Icon { get; } = "IconDateTime";

        #endregion

        #region Fields

        private readonly CultureInfo targetCulture = CultureInfo.CreateSpecificCulture("cs-CZ");

        #endregion

        #region Logic

        public override bool TryRead(string tagType, string value, out DateTime result) {
            if (!string.IsNullOrWhiteSpace(value)) {
                value = value.Trim('"');
            }

            if (DateTime.TryParse(value, targetCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime parsedDateTime)) {
                result = parsedDateTime;
                return true;
            }

            result = DateTime.MinValue;
            return false;
        }

        public override bool TryWrite(DateTime value, out string result) {
            result = value.ToString(targetCulture);
            return true;
        }

        public override Task<string> GetText(DateTime value) {
            return Task.FromResult(value.ToString(targetCulture));
        }

        #endregion

    }

}
