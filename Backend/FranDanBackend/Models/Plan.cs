using FranDanBackend.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FranDanBackend.Models
{
    public class Plan
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int admininstratorId { get; set; }
        [ForeignKey(nameof(admininstratorId))]
        public User creator { get; set; }

        public Plan() { }

        public Plan(string _title, string _description, string _startTime, string _endTime, User _admininstrator)
        {
            title = _title;
            description = _description;
            try
            {
                startTime = DateTime.Parse(_startTime, CultureInfo.GetCultureInfo("pl-PL"));
                endTime = DateTime.Parse(_endTime, CultureInfo.GetCultureInfo("pl-PL"));
            }
            catch (Exception) { throw new Exception("Date-exception"); }
            admininstratorId = _admininstrator.id;
            creator = _admininstrator;
        }

        public PlanHeaderDTO toPlanHeaderDTO()
        {
            return new PlanHeaderDTO
            {
                id = id,
                title = title,
                startTime = startTime.ToString("dd.MM.yyyy"),
                endTime = endTime.ToString("dd.MM.yyyy")
            };
        }
    }
}