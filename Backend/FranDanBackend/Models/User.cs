
using FranDanBackend.DTO;
using Microsoft.IdentityModel.SecurityTokenService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Numerics;

namespace FranDanBackend.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public MailAddress email { get; set; }
        public string passwordHash { get; set; }
        public bool emailNotifications { get; set; }
        public int verifierId { get; set; }
        [ForeignKey(nameof(verifierId))]
        public Verifier verifier { get; set; }
        public string occupation {  get; set; }
        public DateOnly birthday { get; set; }
        public User() { }

        public User(string _username, string _email, bool _emailNotifications, string _passwordHash, string _occupation, string _birthday)
        {
            username = _username;
            email = new MailAddress(_email, _username);
            emailNotifications = _emailNotifications;
            passwordHash = _passwordHash;
            occupation = _occupation;
            try { birthday = DateOnly.Parse(_birthday); } catch (Exception) { throw new Exception("Date-exception"); }
            verifier = new Verifier();
        }
        /*
        public void inviteFriend(User user)
        {
            if (friends.Contains(user)) throw new Exception("Already a friend.");
            if (user.friendRequests.Contains(this)) throw new Exception("Request already send.");
            if (user.blackList.Contains(this)) throw new Exception("Your previous invitation was rejected. You are on black list.");
            blackList.Remove(user);
            user.friendRequests.Add(this);
            if (user.emailNotifications == true)
            {
                MailSender.sendFriendRequest(this, user);
            }
        }
        public void acceptFriend(User user)
        {
            if (friends.Contains(user)) throw new Exception("Already a friend.");
            if (!friendRequests.Contains(user)) throw new Exception("There wasn't any invitation.");
            friendRequests.Remove(user);
            friends.Add(user);
            user.friends.Add(this);
        }
        public void rejectFriend(User user)
        {
            if (friends.Contains(user)) throw new Exception("Already a friend.");
            if (!friendRequests.Contains(user)) throw new Exception("There wasn't any invitation.");
            friendRequests.Remove(user);
            blackList.Add(user);
        }
        public void removeFriend(User user)
        {
            if (!friends.Contains(user)) throw new Exception("Not a friend.");
            friends.Remove(user);
            blackList.Add(user);
        }
        */
        public RequestorDTO toRequestorDTO()
        {
            RequestorDTO dto = new RequestorDTO();
            dto.id = id;
            dto.username = username;
            dto.email = email.Address;
            dto.occupation = occupation;
            return dto;
        }
        public UserProtectedDTO toProtectedDTO()
        {
            UserProtectedDTO dto = new UserProtectedDTO();
            dto.id = id;
            dto.username = username;
            dto.email = email.Address;
            dto.occupation = occupation;
            dto.birthday = birthday.ToString("dd.MM.yyyy");
            return dto;
        }
        public PlanMemberDTO toPlanMemberDTO(bool accepted,bool admin)
        {
            PlanMemberDTO dto = new PlanMemberDTO();
            dto.username = username;
            dto.occupation = occupation;
            dto.accepted = accepted;
            dto.admin = admin;
            return dto;
        }
        /*
        public UserFullDTO toUserFullDTO()
        {
            UserFullDTO dto=new UserFullDTO();
            dto.id = id;
            dto.username = username;
            dto.email = email.Address;
            dto.birthday = birthday.ToString("dd.MM.yyyy");
            dto.friends = new List<UserProtectedDTO>();
            foreach (User friend in friends)
            {
                dto.friends.Add(friend.toProtectedDTO());
            }
            dto.friendRequests = new List<UserProtectedDTO>();
            foreach (User requestor in friendRequests)
            {
                dto.friends.Add(requestor.toProtectedDTO());
            }
            dto.plans = new List<PlanHeaderDTO>();
            foreach (Plan plan in plans)
            {
                dto.plans.Add(plan.toPlanHeaderDTO());
            }
            return dto;
        }
        */
        /*
        public Plan createPlan(string _title, string _description, string _startTime, string _endTime)
        {
            return new Plan(_title, _description, _startTime, _endTime,this);
        }
        public void acceptPlan(Plan plan)
        {
            plan.accept(this);
        }
        public void rejectPlan(Plan plan)
        {
            plan.reject(this);
        }
        */
    }
}
