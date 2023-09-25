using Assignment4Final.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Exams;

namespace Assignment4Final.Services;

public class NotCandidateExamService
{
    private ApplicationDbContext _context;

    public NotCandidateExamService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Candidate GetCandidateByUserId(string userId)
    {
        return _context.Candidates.Where(cand => cand.AppUser.Id == userId)
            .FirstOrDefault();
    }

    public List<Exam> GetAllExams()
    {
        _context.Exams
            .ToList()
            .ForEach(exam =>
                _context.Entry(exam).Reference(exam1 => exam1.Certificate).Load());
        return _context.Exams.ToList();
    }

    public List<SelectListItem> GetExamsSelectList(List<Exam> examsList)
    {
        if (examsList != null && examsList.Count() > 0)
        {
            var examsSelectList = new List<SelectListItem>();
            var group = new SelectListGroup();
            foreach (var exam in examsList)
            {
                var selectListItem = new SelectListItem()
                {
                    Disabled = false,
                    Group = null,
                    Selected = false,
                    Text = $"Id:{exam.Id}, {exam.Certificate.Title}",
                    Value = exam.Id.ToString()
                };
                examsSelectList.Add(selectListItem);
            }

            return examsSelectList;
        }

        return null;
    }
}
