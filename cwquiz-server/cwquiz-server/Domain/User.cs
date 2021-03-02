namespace CW.Thiedze.Domain
{
    public class User : CwQuizTableEntity
    {
        public string Username { get; set; }
        public bool IsValid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Token { get; set; }

        public User(bool isValid)
        {
            IsValid = isValid;
        }

        public User() { }
    }
}