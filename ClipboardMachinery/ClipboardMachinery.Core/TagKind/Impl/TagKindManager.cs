using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace ClipboardMachinery.Core.TagKind.Impl {

    public class TagKindManager : ITagKindManager {

        #region Properties

        public ILogger Logger { get; set; } = NullLogger.Instance;

        public IReadOnlyList<ITagKindSchema> Schemas { get; }

        #endregion

        #region Fields

        private readonly Dictionary<Type, ITagKindSchema> schemaMap;

        #endregion

        public TagKindManager(ITagKindSchemaFactory kindSchemaFactory, ILogger logger) {
            ITagKindSchema[] schemas = kindSchemaFactory.GetAll();

            Logger = logger;
            Logger.Info("Listing available tag kind schema:");
            foreach (ITagKindSchema tagKindSchema in schemas) {
                Logger.Info($" - Name={tagKindSchema.Name}, Type={tagKindSchema.GetType().FullName}");
            }

            Schemas = Array.AsReadOnly(schemas.Reverse().ToArray());
            schemaMap = schemas.ToDictionary(sch => sch.Kind, sch => sch);
        }

        #region Logic

        public ITagKindSchema GetSchemaFor(Type tagKind) {
            return schemaMap.ContainsKey(tagKind)
                ? schemaMap[tagKind]
                : null;
        }

        public object Read(string tagType, Type tagKind, string value) {
            ITagKindSchema tagKindSchema = GetSchemaFor(tagKind);

            if (tagKindSchema == null) {
                return null;
            }

            tagKindSchema.TryRead(tagType, value, out object result);
            return result;
        }

        public bool IsValid(string tagType, Type tagKind, string value) {
            ITagKindSchema tagKindSchema = GetSchemaFor(tagKind);
            return tagKindSchema != null && tagKindSchema.TryRead(tagType, value, out object _);
        }

        public Task<string> GetText(Type tagKind, object value) {
            ITagKindSchema tagKindSchema = GetSchemaFor(tagKind);
            return tagKindSchema == null ? Task.FromResult(value?.ToString()) : tagKindSchema.GetText(value);
        }

        #endregion

    }

}
