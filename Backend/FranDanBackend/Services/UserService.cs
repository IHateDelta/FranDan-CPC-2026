using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.SecurityTokenService;
using System.IO;
using System.Numerics;

namespace FranDanBackend.Services
{
    public class UserService
    {
        public MyContext context { get; set; }
        public UserService(MyContext _context)
        {
            context = _context;
        }
        public UserFullDTO getUserFullDTO(int userId) {
            User user = context.getUserById(userId);

            List<User> friendsList = context.getAllFriends(user);
            List<UserProtectedDTO> friendsListDTO = new List<UserProtectedDTO>();
            foreach (User friend in friendsList) {
                friendsListDTO.Add(friend.toProtectedDTO());
            }

            List<User> friendInvitationsList = context.getAllFriendInvitators(user);
            List<UserProtectedDTO> friendInvitationsListDTO = new List<UserProtectedDTO>();
            foreach (User friend in friendInvitationsList)
            {
                friendInvitationsListDTO.Add(friend.toProtectedDTO());
            }

            var plansList = context.getAllPlans(user);
            List<PlanHeaderDTO> plansListDTO = new List<PlanHeaderDTO>();
            foreach (var (plan, creator, admin) in plansList)
            {
                plansListDTO.Add(plan.toPlanHeaderDTO(creator, admin));
            }

            var planInvitationsList = context.getAllPlanInvitations(user);
            List<PlanHeaderDTO> planInvitationsListDTO = new List<PlanHeaderDTO>();
            foreach (var (plan, admin) in planInvitationsList)
            {
                planInvitationsListDTO.Add(plan.toPlanHeaderDTO(false, admin));
            }
            return new UserFullDTO {
                id = user.id,
                username = user.username,
                email=user.email.Address,
                occupation=user.occupation,
                birthday=user.birthday.ToString("dd.MM.yyyy"),
                friends=friendsListDTO,
                friendInvitations=friendInvitationsListDTO,
                plans = plansListDTO,
                planInvitations=planInvitationsListDTO
            };
        }
        
    }
}
