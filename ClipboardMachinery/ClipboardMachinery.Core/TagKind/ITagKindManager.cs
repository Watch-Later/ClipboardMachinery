using System;
using System.Collections.Generic;

namespace ClipboardMachinery.Core.TagKind {

    public interface ITagKindManager {

        IReadOnlyList<ITagKindSchema> Schemas { get; }

        ITagKindSchema GetSchemaFor(Type kindType);

        bool IsValid(Type kindType, string value);

        object Read(Type kindType, string value);

        string GetText(Type kind, object value);

    }

}
