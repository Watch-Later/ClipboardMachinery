using ServiceStack.DataAnnotations;

namespace ClipboardMachinery.Core.DataStorage.Schema {

    public class EnumType {

        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public string Value { get; set; }

    }

}
