using System;
using System.Linq;
using System.Reflection;

namespace Template.Domain.ValueObjects.Base
{
    public class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        #region IEquatable and Equals operators overrides
        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            if (object.Equals(left, null))
            {
                return object.Equals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }

        ////public bool Equals(TValueObject other)
        ////{
        ////    if ((object)other == null)
        ////    {
        ////        return false;
        ////    }

        ////    if (ReferenceEquals(this, other))
        ////    {
        ////        return true;
        ////    }

        ////    // Compare all public properties
        ////    var publicProperties = GetType().GetProperties();
        ////    var publicFields = GetType().GetFields();

        ////    if (publicProperties.Any())
        ////    {
        ////        return publicProperties.ToList().All(p =>
        ////        {
        ////            var left = p.GetValue(this, null);
        ////            var right = p.GetValue(other, null);

        ////            if (left is TValueObject)
        ////            {
        ////                // Check not self-references...
        ////                return ReferenceEquals(left, right);
        ////            }

        ////            return left.Equals(right);
        ////        });
        ////    }

        ////    // Compare all public fields
        ////    if (publicFields.Any())
        ////    {
        ////        return publicFields.ToList().All(p =>
        ////        {
        ////            var left = p.GetValue(this);
        ////            var right = p.GetValue(other);

        ////            if (left is TValueObject)
        ////            {
        ////                // Check not self-references...
        ////                return ReferenceEquals(left, right);
        ////            }

        ////            return left.Equals(right);
        ////        });
        ////    }

        ////    return true;
        ////}

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = obj as ValueObject<TValueObject>;

            return (object)item != null && this.Equals((TValueObject)item);
        }

        public bool Equals(TValueObject other)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            const int INDEX = 1;
            var hashCode = 31;
            var changeMultiplier = false;

            // Compare all public properties.
            var publicProperties = GetType().GetProperties();

            if (publicProperties.Any())
            {
                foreach (var prop in publicProperties)
                {
                    var value = prop.GetValue(this, null);

                    if (value != null)
                    {
                        hashCode = hashCode * ((changeMultiplier ? 59 : 114) + value.GetHashCode());
                        changeMultiplier = !changeMultiplier;
                    }
                    else
                    {
                        // Only for support {"a",null,null,"a"} <> {null,"a","a",null}
                        hashCode = hashCode ^ (INDEX * 13);
                    }
                }
            }

            return hashCode;
        }
        #endregion
    }
}