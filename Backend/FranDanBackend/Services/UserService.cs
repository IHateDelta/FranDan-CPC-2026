using F1ProjKredek;
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
        public void addUser(AuthRegisterDTO dto)
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
    }
}
