namespace PasswordGenerator.Models
{
    public class RequestModel
    {
        public int PasswordLength { get; set; }

        public int PasswordNumber { get; set; }

        public bool IsHaveNumericalCharacter { get; set; } = true;

        public int MinimumNumberOfNumericalCharacter { get; set; } = 0;

        public bool IsHaveUpperCaseLetter { get; set; } = true;

        public int MinimumNumberOfUpperCaseLetter { get; set; } = 0;

        public bool IsHaveLowerCaseLetter { get; set; } = true;

        public int MinimumNumberOfLowerCaseLetter { get; set; }

        public bool IsHaveSpecialCharacter { get; set; } = true ;

        public int MinimumNumberOfSpecialCharacter { get; set; } = 0;

        public string? MustHave { get; set; }

        public string? MustStartsWith { get; set; }

        public string? MustEndsWith { get; set; }

        public string? CanContain { get; set; }

        public string? CanNotContain { get; set; }

        public string ReturnStyle { get; set; }

        public RequestModel Clone()
        {
            return (RequestModel)this.MemberwiseClone();
        }
    }
}
