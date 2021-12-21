using System;
using System.Threading.Tasks;

namespace ClipboardMachinery.Core.TagKind {

    public interface ITagKindSchema {

        #region Properties

        Type Kind { get; }

        string Name { get; }

        string Description { get; }

        string Icon { get; }

        #endregion

        bool TryRead(string tagType, string value, out object result);

        bool TryWrite(object value, out string result);

        Task<string> GetText(object value);

    }

    public abstract class TagKindSchema<TKind> : ITagKindSchema {

        #region Properties

        public Type Kind { get; } = typeof(TKind);

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Icon { get; }

        #endregion

        bool ITagKindSchema.TryRead(string tagType, string value, out object result) {
            bool wasParsed = TryRead(tagType, value, out TKind typedResult);
            result = typedResult;
            return wasParsed;
        }

        bool ITagKindSchema.TryWrite(object value, out string result) {
            if (value is TKind typedValue) {
                return TryWrite(typedValue, out result);
            }

            result = null;
            return false;
        }

        Task<string> ITagKindSchema.GetText(object value) {
            if (value is TKind typedValue) {
                return GetText(typedValue);
            }

            return Task.FromResult(value?.ToString());
        }

        public abstract bool TryRead(string tagType, string value, out TKind result);

        public abstract bool TryWrite(TKind value, out string result);

        public abstract Task<string> GetText(TKind value);

    }

}
