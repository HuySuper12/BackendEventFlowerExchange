using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IRequestRepository
    {
        public Task<bool> CreateRequestAsync(CreateRequest value);

        public Task<bool> UpdateRequestAsync(Request value);

        public Task<List<Request?>> GetListRequestsAsync(string value);

        public Task<Request?> GeRequestByIdAsync(int id);

        public Task<Request?> GetRequestByProductIdAsync(int id);

        public Task<List<Request?>> GetListRequestsByEmailAndTypeAsync(string type, Account account);


    }
}
