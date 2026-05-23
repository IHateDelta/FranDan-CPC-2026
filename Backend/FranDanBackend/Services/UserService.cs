using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.Mail;

namespace FranDanBackend.Services
{
    public class UserService
    {
        public MyContext context {  get; set; }
        public JWTGenerator generator { get; set; }
        public UserService(MyContext _context, JWTGenerator _generator)
        {
            context = _context;
            generator = _generator;
        }
        public void add(AuthRegisterDTO dto)
        {
            Console.WriteLine("I'm here!");
            string targetEmail = dto.email.Trim().ToLower();
            string targetUsername = dto.username.Trim().ToLower();
            var searchAddress = new MailAddress(targetEmail);

            // 1. POPRAWIONE: EF Core użyje teraz konwertera 'mailAddressConverter', 
            // zamieni searchAddress na string i porówna go w bazie danych bez żadnych błędów castowania!
            bool emailExists = context.Users.Any(user => user.email == searchAddress);

            if (emailExists)
                throw new Exception("User with this email exists.");
            Console.WriteLine("Done!");
            if (emailExists)
                throw new Exception("User with this email exists.");
            if (context.Users.Any(user => string.Equals(user.username.ToLower(), targetUsername)))
                throw new Exception("User with this username exists.");
            User newUser =new User(
                dto.username,
                dto.email,
                dto.emailNotifications,
                JWTGenerator.createHash(dto.password),
                dto.birthday
                );
            Console.WriteLine("I'll send mail!");
            MailSender.sendCode(newUser);
            context.Users.Add(newUser);
            Console.WriteLine("Good!");
            context.SaveChanges();
        }
        public void verify(AuthVerifyDTO dto)
        {
            User foundUser = context.Users
                .Include(u => u.verifier)
                .FirstOrDefault(user => user.username.ToLower() == dto.usernameOrEmail.ToLower());
                
            if (foundUser == null) try {
                    var searchAddress = new MailAddress(dto.usernameOrEmail.ToLower());
                    foundUser = context.Users
                        .Include(u => u.verifier)
                        .FirstOrDefault(user => user.email == searchAddress); } catch { }
            if (foundUser == null) throw new Exception("No user found!");
            if (foundUser.verifier.verified) throw new Exception("You are already verified! Log in!");
            if (!foundUser.verifier.verify(dto.code)) throw new Exception("Wrong code!");
            context.SaveChanges();
        }
        public string login(AuthLoginDTO dto)
        {
            User foundUser = context.Users
                .Include(u => u.verifier)
                .FirstOrDefault(user => user.username.ToLower() == dto.usernameOrEmail.ToLower());

            if (foundUser == null) try
                {
                    var searchAddress = new MailAddress(dto.usernameOrEmail.ToLower());
                    foundUser = context.Users
                        .Include(u => u.verifier)
                        .FirstOrDefault(user => user.email == searchAddress);
                }
                catch { }
            if (foundUser == null) throw new Exception("No user found!");
            if (!foundUser.verifier.verified) throw new Exception("Verify email before loging in!");
            if (!JWTGenerator.verifyHash(dto.password,foundUser.passwordHash)) throw new Exception("Wrong password!");
            return generator.GenerateJWTToken(foundUser);
        }
        /*
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
        */
    }
}
