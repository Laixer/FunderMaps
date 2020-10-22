using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Base for all entities.
    /// </summary>
    public abstract class BaseEntity<TEntity> : IEquatable<TEntity>, IComparable<TEntity>, IInitializable<TEntity>
        where TEntity : class
    {
        /// <summary>
        ///     Get derived entity.
        /// </summary>
        protected TEntity DerivedEntity => this as TEntity;

        /// <summary>
        ///     Whether the entity passed validation or not.
        /// </summary>
        public bool IsValidated { get; private set; }

        /// <summary>
        ///     Validate entity properties.
        /// </summary>
        /// <remarks>
        ///     If the validation is passed once skip in all proceeding calls.
        /// </remarks>
        public virtual void Validate()
        {
            if (!IsValidated)
            {
                ValidateOject();
                IsValidated = true;
            }
        }

        /// <summary>
        ///     Let the object initliaze defaults after the instance was created.
        /// </summary>
        public virtual void InitializeDefaults()
        {
        }

        /// <summary>
        ///     Let the object initliaze defaults from another entity of the same type.
        /// </summary>
        public virtual void InitializeDefaults(TEntity other)
        {
        }

        // FUTURE: validate navigation properties as well
        /// <summary>
        ///     Validate the object data properties. This throws an exception if one or
        ///     more validation rules are voilated.
        /// </summary>
        /// <exception cref="ValidationException">Thrown when validation rules are voilated.</exception>
        protected virtual void ValidateOject()
            => Validator.ValidateObject(DerivedEntity, new ValidationContext(DerivedEntity), true);

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, false.</returns>
        public abstract bool Equals(TEntity other);

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns
        ///     an integer that indicates whether the current instance precedes, follows, or
        ///     occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public abstract int CompareTo(TEntity other);

        #region Operator Overloading

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj != null && Equals(obj as TEntity);

        /// <summary>
        ///     Compare left smaller than right.
        /// </summary>
        /// <param name="left">Instance of <see cref="BaseEntity{TEntity}"/>.</param>
        /// <param name="right">Instance of type <typeparamref name="TEntity"/>.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public static bool operator <(BaseEntity<TEntity> left, TEntity right)
            => left != null && left.CompareTo(right) < 0;

        /// <summary>
        ///     Compare left smaller or equal than right.
        /// </summary>
        /// <param name="left">Instance of <see cref="BaseEntity{TEntity}"/>.</param>
        /// <param name="right">Instance of type <typeparamref name="TEntity"/>.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public static bool operator <=(BaseEntity<TEntity> left, TEntity right)
            => left != null && left.CompareTo(right) <= 0;

        /// <summary>
        ///     Compare left greater than right.
        /// </summary>
        /// <param name="left">Instance of <see cref="BaseEntity{TEntity}"/>.</param>
        /// <param name="right">Instance of type <typeparamref name="TEntity"/>.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public static bool operator >(BaseEntity<TEntity> left, TEntity right)
            => left != null && left.CompareTo(right) > 0;

        /// <summary>
        ///     Compare left greater or equal than right.
        /// </summary>
        /// <param name="left">Instance of <see cref="BaseEntity{TEntity}"/>.</param>
        /// <param name="right">Instance of type <typeparamref name="TEntity"/>.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public static bool operator >=(BaseEntity<TEntity> left, TEntity right)
            => left != null && left.CompareTo(right) >= 0;

        /// <summary>
        ///     Compare left equal to right.
        /// </summary>
        /// <param name="left">Instance of <see cref="BaseEntity{TEntity}"/>.</param>
        /// <param name="right">Instance of type <typeparamref name="TEntity"/>.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public static bool operator ==(BaseEntity<TEntity> left, BaseEntity<TEntity> right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        ///     Compare left not equal to right.
        /// </summary>
        /// <param name="left">Instance of <see cref="BaseEntity{TEntity}"/>.</param>
        /// <param name="right">Instance of type <typeparamref name="TEntity"/>.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public static bool operator !=(BaseEntity<TEntity> left, BaseEntity<TEntity> right)
            => !(left == right);

        #endregion Operator Overloading
    }

    /// <summary>
    ///     Base for all indentifiable entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TEntryIdentifier">Entity identifier type.</typeparam>
    public abstract class IdentifiableEntity<TEntity, TEntryIdentifier> : BaseEntity<TEntity>
        where TEntity : class
        where TEntryIdentifier : IEquatable<TEntryIdentifier>, IComparable<TEntryIdentifier>
    {
        /// <summary>
        ///     Function will return the <typeparamref name="TEntity"/> identifier.
        /// </summary>
        protected Func<TEntity, TEntryIdentifier> EntityIdentifier { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IdentifiableEntity(Func<TEntity, TEntryIdentifier> entryPrimaryKey)
            => EntityIdentifier = entryPrimaryKey;

        /// <summary>
        ///     <typeparamref name="TEntity"/> identifier.
        /// </summary>
        public TEntryIdentifier Identifier
            => EntityIdentifier(DerivedEntity);

        /// <summary>
        ///     Print object as identifier.
        /// </summary>
        /// <returns>String representing entity identifier.</returns>
        public override string ToString()
            => Identifier.ToString();

        /// <summary>
        ///     Check if self is equal to other entity.
        /// </summary>
        /// <param name="other">Entity to compare.</param>
        /// <returns><c>True</c> on success, false otherwise.</returns>
        public override bool Equals(TEntity other)
            => other != null && Identifier.Equals(EntityIdentifier(other));

        /// <summary>
        ///     Compare self to other entity.
        /// </summary>
        /// <param name="other">Entity to compare.</param>
        /// <returns>The Levenshtein distance between objects.</returns>
        public override int CompareTo(TEntity other)
            => other == null ? 1 : Identifier.CompareTo(EntityIdentifier(other));
    }
}
