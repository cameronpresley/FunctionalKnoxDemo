using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalKnox.Domain;
using Optionally;
using Optionally.Extensions;

namespace FunctionalKnox.DataAccess
{
    public class WorkItemRepository : IRepository<WorkItem>
    {
        private readonly List<WorkItem> _items;

        public WorkItemRepository()
        {
            _items = new List<WorkItem>();
        }

        public IResult<Exception, IEnumerable<WorkItem>> GetAll()
        {
            return Result.Success<Exception, IEnumerable<WorkItem>>((IEnumerable<WorkItem>)_items);
        }

        public IResult<Exception, IOption<WorkItem>> GetById(int id)
        {
            return Result.Success<Exception, IOption<WorkItem>>
                         (_items.TryFirst(x => x.Id == id));
        }

        public IResult<Exception, bool> Delete(WorkItem item)
        {
            try
            {
                var recordsRemoved = _items.RemoveAll(x => x.Id == item.Id);
                return recordsRemoved == 0
                    ? Result.Success<Exception, bool>(false)
                    : Result.Success<Exception, bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<Exception, bool>(ex);
            }
        }

        public IResult<Exception, WorkItem> Add(WorkItem item)
        {
            if (_items.Contains(item))
                return Result.Failure<Exception, WorkItem>(new Exception("Record has already been added."));

            var id = GenerateNewId();
            item.Id = id;
            _items.Add(item);
            return Result.Success<Exception, WorkItem>(item);
        }

        private int GenerateNewId()
        {
            return !_items.Any()
                ? 1
                : _items.Max(x => x.Id) + 1;
        }
    }
}
