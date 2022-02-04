namespace PasswordGeneratorOld.Models
{
    public class RequestModel
    {
        public int Length { get; set; } = 16;

        public int Count { get; set; } = 1;

        public int Numbers { get; set; } = 0;

        public int UpperChars { get; set; } = 0;

        public int LowerChars { get; set; } = 0;

        public int SpecialChars { get; set; } = 1;

        public string? MustHave { get; set; } = null;

        public string? StartsWith { get; set; } = null;

        public string? EndsWith { get; set; } = null;

        public string? Include { get; set; } = null;

        public string? Exclude { get; set; } = null;

        public string Type { get; set; } = "";

        public RequestModel Clone()
        {
            return (RequestModel)this.MemberwiseClone();
        }
    }
}
