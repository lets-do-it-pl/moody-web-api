using System;
using System.Net.Mail;

namespace LetsDoIt.Moody.Domain.ValueType
{
    public readonly struct Email
    {
        private readonly string _value;

        public Email(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Email can not be null!");
            }

            if (!IsValid(value))
            {
                throw new ArgumentException("Invalid email address.", value);
            }

            _value = value.ToLowerInvariant();
        }

        public static bool IsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email);

                return string.Equals(mailAddress.Address, email, StringComparison.InvariantCultureIgnoreCase);
            }
            catch
            {
                return false;
            }

        }

        public static bool TryParse(string candidate, out Email? email)
        {
            email = null;

            if (string.IsNullOrWhiteSpace(candidate))
            {
                return false;
            }

            email = new Email(candidate);

            return true;
        }

        public static Email Parse(string candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                throw new ArgumentException("Email can not be empty!");
            }

            return new Email(candidate);
        }

        public static implicit operator string(Email email)
        {
            return email._value;
        }

        public static implicit operator Email(string email)
        {
            return new Email(email);
        }

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Email objEmail)
            {
                return string.Equals(this._value, objEmail._value, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}