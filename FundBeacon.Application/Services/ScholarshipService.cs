using FundBeacon.Application.Interfaces;
using FundBeacon.Data;
using FundBeacon.Domain.Models;
using FundBeacon.Dto;
using System.Data.Entity;


namespace FundBeacon.Application.Services
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly FundBeaconDbContext _context;

        public ScholarshipService(FundBeaconDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Scholarship> CreateScholarshipAsync(ScholarshipDto dto)
        {
            var provider = await _context.ScholarshipProviders.FindAsync(dto.ProviderId);
            if (provider == null)
            {
                throw new ArgumentException("Scholarship provider not found.");
            }

            var scholarship = new Scholarship
            {
                Title = dto.Title,
                About = dto.About,
                Eligibility = dto.Eligibility,
                Deadline = dto.Deadline,
                Benefits = dto.Benefits,
                DocumentsRequired = dto.DocumentsRequired,
                HowCanYouApply = dto.HowCanYouApply,
                ContactUs = dto.ContactUs,
                Disclaimer = dto.Disclaimer,
                ProviderId = dto.ProviderId
            };

            _context.Scholarships.Add(scholarship);
            await _context.SaveChangesAsync();

            return scholarship;
        }


        public Task<Scholarship> GetScholarshipByIdAsync(int id)
        {
            var scholarship = _context.Scholarships
         .Include(s => s.Provider)
         .Include(s => s.Applications)
         .FirstOrDefault(s => s.ScholarshipId == id && !s.IsDeleted);

            return Task.FromResult(scholarship);
        }

        public Task<List<Scholarship>> GetAllScholarshipsAsync()
        {
            var result = _context.Scholarships
       .Include(s => s.Provider)
       .Include(s => s.Applications)
       .Where(s => !s.IsDeleted) 
       .ToList();

            return Task.FromResult(result);
        }

        public async Task<Scholarship> UpdateScholarshipAsync(int id, ScholarshipDto dto)
        {
            var scholarship = _context.Scholarships.FirstOrDefault(s => s.ScholarshipId == id);
            if (scholarship == null)
                throw new KeyNotFoundException("Scholarship not found.");

            // Update fields
            scholarship.Title = dto.Title;
            scholarship.About = dto.About;
            scholarship.Eligibility = dto.Eligibility;
            scholarship.Deadline = dto.Deadline;
            scholarship.Benefits = dto.Benefits;
            scholarship.DocumentsRequired = dto.DocumentsRequired;
            scholarship.HowCanYouApply = dto.HowCanYouApply;
            scholarship.ContactUs = dto.ContactUs;
            scholarship.Disclaimer = dto.Disclaimer;
            scholarship.ProviderId = dto.ProviderId;

            await _context.SaveChangesAsync();

            return scholarship;
        }

        public async Task<bool> DeleteScholarshipAsync(int id)
        {
            var scholarship = _context.Scholarships.FirstOrDefault(s => s.ScholarshipId == id);
            if (scholarship == null)
                return false;

            scholarship.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
