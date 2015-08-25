using System;

namespace Lx.Tools.Common
{
    public class Option : IEquatable<Option>
    {
        public string Name { get; set; }
        public string Explanation { get; set; }

        public bool Equals(Option other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Option) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Option left, Option right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Option left, Option right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}