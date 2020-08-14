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

        public virtual void InitializeDefaults()
        {
        }

        public virtual void InitializeDefaults(TEntity other)
        {
        }

        protected virtual void ValidateOject()
            => Validator.ValidateObject(DerivedEntity, new ValidationContext(DerivedEntity), true);

        public abstract bool Equals(TEntity other);
        public abstract int CompareTo(TEntity other);

        #region Operator Overloading

        public override bool Equals(object obj)
            => obj != null && Equals(obj as TEntity);

        public static bool operator <(BaseEntity<TEntity> left, TEntity right)
            => left.CompareTo(right) < 0;

        public static bool operator <=(BaseEntity<TEntity> left, TEntity right)
            => left.CompareTo(right) <= 0;

        public static bool operator >(BaseEntity<TEntity> left, TEntity right)
            => left.CompareTo(right) > 0;

        public static bool operator >=(BaseEntity<TEntity> left, TEntity right)
            => left.CompareTo(right) >= 0;

        public static bool operator ==(BaseEntity<TEntity> left, BaseEntity<TEntity> right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);

        public static bool operator !=(BaseEntity<TEntity> left, BaseEntity<TEntity> right)
            => !(left == right);

        #endregion Operator Overloading
    }

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
        ///     Print object as primary key.
        /// </summary>
        /// <returns>String representing entity primary key.</returns>
        public override string ToString()
            => EntityIdentifier(DerivedEntity).ToString();

        public override bool Equals(TEntity other)
            => other != null && EntityIdentifier(DerivedEntity).Equals(EntityIdentifier(other));

        public override int CompareTo(TEntity other)
            => other == null ? 1 : EntityIdentifier(DerivedEntity).CompareTo(EntityIdentifier(other));
    }
}
