
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Net.Mail;
namespace FranDanBackend.Models{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public MailAddress email { get; set; }
        public string passwordHash { get; set; }
        public bool emailNotifications { get; set; }
        public Verifier verifier { get; set; }
        public DateOnly birthday {  get; set; }
        public HashSet<User> friends {  get; set; }
        public HashSet<User> friendRequests { get; set; }
        public HashSet<User> blackList { get; set; }
        public HashSet<Plan> plans { get; set; }
        public HashSet<Plan> planRequests { get; set; }
        public User() { }

        public User(string _username, string _email, bool _emailNotifications, string _passwordHash, string _birthday)
        {
            username = _username;
            email = new MailAddress(_email,username);
            emailNotifications = _emailNotifications;
            passwordHash = _passwordHash;
            verifier = new Verifier();
            try { birthday = DateOnly.Parse(_birthday); }catch (Exception) { throw new Exception("Date-exception"); }
            friends = [];
            friendRequests = new HashSet<User>();
            blackList = new HashSet<User>();
            plans = new HashSet<Plan>();
            planRequests = new HashSet<Plan>();
        }
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
