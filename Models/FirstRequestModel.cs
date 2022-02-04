namespace PasswordGeneratorOld.Models
{
    public class FirstRequestModel
    {
        public string? Length { get; set; } = null;

        public string? Count { get; set; } = null;

        public string? Numbers { get; set; } = null;

        public string? UpperChars { get; set; } = null;

        public string? LowerChars { get; set; } = null;

        public string? SpecialChars { get; set; } = null;

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
