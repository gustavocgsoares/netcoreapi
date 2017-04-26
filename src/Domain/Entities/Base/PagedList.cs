using System.Collections.Generic;

namespace Template.Domain.Entities.Base
{
    public class PagedList<TEntity>
    {
        #region Constructors | Destructors
        public PagedList(int limit = 10)
        {
            Limit = limit;
        }
        #endregion

        #region Properties
        public int Offset { get; set; }

        public int Limit { get; set; }

        public ICollection<TEntity> Items { get; set; }

        public long Total { get; set; }
        #endregion
    }
}