
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Numerics;

namespace FranDanBackend.Models {
    public class Plan
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public User admininstrator { get; set; }
        public Dictionary<User, (bool,bool)> participants { get; set; }
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
            admininstrator = _admininstrator;
            participants = new Dictionary<User, (bool,bool)>();
            addParticipant(admininstrator, true,true);
        }
        public void addParticipant(User participant, bool accepted=false, bool admin = false)
        {
            participants.Add(participant, (accepted,admin));
            if(accepted)
                participant.plans.Add(this);
            else
                participant.planRequests.Add(this);
        }
        public void removeParticipant(User participant) {
            participants.Remove(participant);
            participant.planRequests.Remove(this);
            participant.plans.Remove(this);
        }
        public void setAdmin(User participant,bool admin)
        {
            participants[participant] = (participants[participant].Item1,admin);
        }
        public void accept(User participant)
        {
            bool admin = participants[participant].Item1;
            removeParticipant(participant);
            addParticipant(participant, true, admin);
        }
        public void reject(User participant)
        {
            removeParticipant(participant);
        }
    }
}
