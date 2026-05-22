
namespace FranDanBackend.DTO{
    public class UserFullDTO
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public List<UserProtectedDTO> friends { get; set; }
        public List<UserProtectedDTO> friendRequests { get; set; }
        public List<PlanHeaderDTO> plans { get; set; }
    }
}
