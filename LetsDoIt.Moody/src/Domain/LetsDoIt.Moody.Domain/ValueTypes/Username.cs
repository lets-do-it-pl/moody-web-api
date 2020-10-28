using System;

namespace LetsDoIt.Moody.Domain.ValueTypes
{
    public class Username
    {
        private readonly string value;

        public Username(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (!Username.IsValid(value))
                throw new ArgumentException("Invalid value.", "value");

            this.value = value;
        }

        public static bool IsValid(string candidate)
        {
            if (string.IsNullOrEmpty(candidate))
                return false;

            return candidate.Trim().ToUpper() == candidate;
        }

        public static bool TryParse(string candidate, out Username username)
        {
            username = null;
            if (string.IsNullOrWhiteSpace(candidate))
                return false;

            username = new Username(candidate.Trim().ToUpper());
            return true;
        }

        public static implicit operator string(Username username)
        {
            return username.value;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Username;
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
