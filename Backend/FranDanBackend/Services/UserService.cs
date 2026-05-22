using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FranDanBackend.Services
{
    public class UserService
    {
        public MyContext context {  get; set; }
        public UserService(MyContext _context)
        {
            context = _context;
        }
        public void add(AuthRegisterDTO dto)
        {
            if (context.Users.Any(user => user.email.Address.ToLower() == dto.email.ToLower()))
                return;
            if(context.Users.Any(user => user.username.ToLower() == dto.username.ToLower()))
                throw new Exception("User with this username exists.");
            User newUser =new User(
                dto.username,
                dto.email,
                dto.emailNotifications,
                JWTGenerator.createHash(dto.password),
                dto.birthday
                );
            context.Users.Add(newUser);
            context.SaveChanges();
        }
        public void verify(AuthVerifyDTO dto)
        {
            User foundUser=context.Users.FirstOrDefault(user => user.email.Address.ToLower() == dto.usernameOrEmail.ToLower());
            if (foundUser == null) foundUser = context.Users.FirstOrDefault(user => user.username.ToLower() == dto.usernameOrEmail.ToLower());
            if (foundUser == null) throw new Exception("No user found!");
            if (!foundUser.verifier.verify(dto.code)) throw new Exception("Wrong code!");
            context.SaveChanges();
        }
        public void inviteFriend(int invitorId,UserFindDTO dto)
        {
            User invitorUser = context.Users.Find(invitorId);
            if (invitorUser == null) throw new Exception("No user found!");
            User invitedUser = context.Users.FirstOrDefault(user => user.email.Address.ToLower() == dto.usernameOrEmail.ToLower());
            if (invitedUser == null) invitedUser = context.Users.FirstOrDefault(user => user.username.ToLower() == dto.usernameOrEmail.ToLower());
            if (invitedUser == null) throw new Exception("No user found!");
            invitorUser.inviteFriend(invitedUser);
            context.SaveChanges();
        }
        public void acceptFriend(int invitorId, UserIdDTO dto)
        {
            User invitedUser = context.Users.Find(invitorId);
            if (invitedUser == null) throw new Exception("No user found!");
            User invitorUser = context.Users.Find(dto.id);
            if (invitorUser == null) throw new Exception("No user found!");
            invitedUser.inviteFriend(invitorUser);
            context.SaveChanges();
        }
        public void rejectFriend(int invitorId, UserIdDTO dto)
        {
            User invitedUser = context.Users.Find(invitorId);
            if (invitedUser == null) throw new Exception("No user found!");
            User invitorUser = context.Users.Find(dto.id);
            if (invitorUser == null) throw new Exception("No user found!");
            invitedUser.rejectFriend(invitorUser);
            context.SaveChanges();
        }
        public void removeFriend(int invitorId, UserIdDTO dto)
        {
            User removerUser = context.Users.Find(invitorId);
            if (removerUser == null) throw new Exception("No user found!");
            User removedUser = context.Users.Find(dto.id);
            if (removedUser == null) throw new Exception("No user found!");
            removerUser.removeFriend(removedUser);
            context.SaveChanges();
        }
        public UserFullDTO fullInfo(int userId)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            return user.toUserFullDTO();

        }
    }
}
