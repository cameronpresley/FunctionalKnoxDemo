using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalKnoxDemo.Models;
using Optionally;
using Optionally.Extensions;

namespace FunctionalKnoxDemo.DataAccess
{
    public interface IRepository<T>
    {
        IResult<Exception, IEnumerable<T>> GetAll();
        IResult<Exception, IOption<T>> GetById(int id);
        IResult<Exception, bool> Delete(T item);
        IResult<Exception, T> Add(T item);
    }

    public class WorkItemRepository : IRepository<WorkItem>
    {
        private readonly List<WorkItem> _workItems;

        public WorkItemRepository()
        {
            _workItems = new List<WorkItem>();
        }

        public IResult<Exception, IEnumerable<WorkItem>> GetAll()
        {
            return Result.Success<Exception, IEnumerable<WorkItem>>(_workItems);
        }

        public IResult<Exception, IOption<WorkItem>> GetById(int id)
        {
            try
            {
                return Result.Success<Exception, IOption<WorkItem>>(_workItems.TryFirst(x => x.Id == id));
            }
            catch (Exception ex)
            {
                return Result.Failure<Exception, IOption<WorkItem>>(ex);
            }
        }

        public IResult<Exception, bool> Delete(WorkItem item)
        {
            try
            {
                _workItems.RemoveAll(x => x.Id == item.Id);
                return Result.Success<Exception, bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<Exception, bool>(ex);
            }
        }

        public IResult<Exception, WorkItem> Add(WorkItem item)
        {
            try
            {
                if (_workItems.Any(x => x.Id == item.Id))
                {
                    return Result.Failure<Exception, WorkItem>(
                        new Exception("Can't add an item that already exists!!!"));
                }
                else
                {
                    var id = _workItems.Count == 0 ? 0 : _workItems.Max(x => x.Id) + 1;
                    item.Id = id;
                    _workItems.Add(item);
                    return Result.Success<Exception, WorkItem>(item);
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<Exception, WorkItem>(ex);
            }
        }
    }
}
