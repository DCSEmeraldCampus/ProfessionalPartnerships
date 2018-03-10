using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProfessionalPartnerships.Web.Data.Entity;
using ProfessionalPartnerships.Web.Repo.Interface;

namespace ProfessionalPartnerships.Web.Repo
{
    public class Repository<T> : ICore<T>
    {
        public T Add()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
