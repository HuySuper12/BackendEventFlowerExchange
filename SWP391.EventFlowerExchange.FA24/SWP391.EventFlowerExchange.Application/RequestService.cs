using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class RequestService : IRequestService
    {
        private IRequestRepository _service;

        public RequestService(IRequestRepository service)
        {
            _service = service;
        }

        public Task<bool> CreateRequestFromAPIAsync(CreateRequest value)
        {
            return _service.CreateRequestAsync(value);
        }

        public Task<List<Request?>> GetListRequestsFromAPIAsync(string value)
        {
            return _service.GetListRequestsAsync(value);
        }

        public Task<bool> UpdateRequestFromAPIAsync(Request value)
        {
            return _service.UpdateRequestAsync(value);
        }

        public Task<Request?> GetRequestByIdFromAPIAsync(int id)
        {
            return _service.GeRequestByIdAsync(id);
        }

        public Task<Request?> GetRequestByProductIdFromAPIAsync(int id)
        {
            return _service.GetRequestByProductIdAsync(id);
        }

        public Task<List<Request?>> GetListRequestsByEmailAndTypeAsync(string type, Account account)
        {
            return _service.GetListRequestsByEmailAndTypeAsync(type, account);
        }
    }
}
