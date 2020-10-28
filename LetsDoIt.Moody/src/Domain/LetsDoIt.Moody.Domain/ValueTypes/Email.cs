using System;
using System.Text.RegularExpressions;

namespace LetsDoIt.Moody.Domain.ValueTypes
{
    public class Email
    {
        private readonly string value;

        private const string RegexPattern =
            "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

        public Email(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (!Email.IsValid(value))
                throw new ArgumentException("Invalid value.", "value");

            this.value = value;
        }

        public static bool IsValid(string candidate)
        {
            if (string.IsNullOrEmpty(candidate))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(candidate);
                return addr.Address == candidate &&
                       candidate.Trim().ToUpper() == candidate;
            }
            catch
            {
                return false;
            }

        }

        public static bool TryParse(string candidate, out Email email)
        {
            email = null;
            if (string.IsNullOrWhiteSpace(candidate))
                return false;

            email = new Email(candidate.Trim().ToUpper());
            return true;
        }

        public static implicit operator string(Email email)
        {
            return email.value;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Email;
            if (other == null)
                return base.Equals(obj);

            return object.Equals(this.value, other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}
