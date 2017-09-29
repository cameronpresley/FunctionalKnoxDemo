using System;
using System.Collections.Generic;
using Optionally;

namespace FunctionalKnox.DataAccess
{
    public interface IRepository<T>
    {
        IResult<Exception, IEnumerable<T>> GetAll();
        IResult<Exception, IOption<T>> GetById(int id);
        IResult<Exception, bool> Delete(T item);
        IResult<Exception, T> Add(T item);
    }
}
