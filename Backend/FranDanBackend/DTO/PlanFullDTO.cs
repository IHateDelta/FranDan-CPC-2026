
namespace FranDanBackend.DTO{
    public class PlanFullDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string startTime { get; set; }
        public string endTime {  get; set; }
        public PlanMemberDTO administrator { get; set; }
        public List<PlanMemberDTO> participants { get; set; }
    }
}
