using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClipboardMachinery.Core.TagKind {

    public interface ITagKindManager {

        IReadOnlyList<ITagKindSchema> Schemas { get; }

        ITagKindSchema GetSchemaFor(Type tagKind);

        bool IsValid(string tagType, Type tagKind, string value);

        object Read(string tagType, Type tagKind, string value);

        Task<string> GetText(Type tagKind, object value);

    }

}
