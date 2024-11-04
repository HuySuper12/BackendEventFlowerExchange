using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IRequestService
    {
        public Task<bool> CreateRequestFromAPIAsync(CreateRequest value);

        public Task<bool> UpdateRequestFromAPIAsync(Request value);

        public Task<List<Request?>> GetListRequestsFromAPIAsync(string value);

        public Task<Request?> GetRequestByIdFromAPIAsync(int id);

        public Task<Request?> GetRequestByProductIdFromAPIAsync(int id);

        public Task<List<Request?>> GetListRequestsByEmailAndTypeAsync(string type, Account account);
    }
}
