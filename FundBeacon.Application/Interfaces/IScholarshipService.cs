using FundBeacon.Domain.Models;
using FundBeacon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Application.Interfaces
{
    public interface IScholarshipService
    {
        Task<Scholarship> CreateScholarshipAsync(ScholarshipDto dto);
        Task<List<Scholarship>> GetAllScholarshipsAsync();
        Task<Scholarship> GetScholarshipByIdAsync(int id);
        Task<Scholarship> UpdateScholarshipAsync(int id, ScholarshipDto dto);
        Task<bool> DeleteScholarshipAsync(int id);


    }
}
