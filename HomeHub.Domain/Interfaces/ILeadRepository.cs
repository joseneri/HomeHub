using HomeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Interfaces
{
    public interface ILeadRepository
    {
        Task<Lead> AddAsync(Lead lead, CancellationToken ct = default);
    }
}
