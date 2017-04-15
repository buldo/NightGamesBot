namespace Buldo.Ngb.FoxApi
{
    using System;

    public class AcceptedCode : IEquatable<AcceptedCode>
    {
        internal AcceptedCode(string type, string value, string message)
        {
            Type = type;
            Value = value;
            Message = message;
        }

        public string Type { get; }

        public string Value { get; }

        public string Message { get; }

        public bool Equals(AcceptedCode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Type, other.Type) && string.Equals(Value, other.Value) && string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AcceptedCode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(AcceptedCode left, AcceptedCode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AcceptedCode left, AcceptedCode right)
        {
            return !Equals(left, right);
        }
    }
}
