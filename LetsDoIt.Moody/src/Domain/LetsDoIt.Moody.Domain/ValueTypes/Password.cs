using System;

namespace LetsDoIt.Moody.Domain.ValueTypes
{
    public class Password
    {
        private readonly string value;

        public Password(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (!Password.IsValid(value))
                throw new ArgumentException("Invalid value.", "value");

            this.value = value;
        }

        public static bool IsValid(string candidate)
        {
            if (string.IsNullOrEmpty(candidate))
                return false;

            return candidate.Trim().ToUpper() == candidate;
        }

        public static bool TryParse(string candidate, out Password password)
        {
            password = null;
            if (string.IsNullOrWhiteSpace(candidate))
                return false;

            password = new Password(candidate.Trim().ToUpper());
            return true;
        }

        public static implicit operator string(Password password)
        {
            return password.value;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Password;
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
