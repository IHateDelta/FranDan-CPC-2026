using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FranDanBackend.Services
{
    public class PlanService
    {
        public MyContext context {  get; set; }
        public PlanService(MyContext _context)
        {
            context = _context;
        }
        public void addPlan(int userId, PlanAddDTO dto)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            Plan newPlan = new Plan(dto.title,dto.description,dto.startTime,dto.endTime,user);
            context.SaveChanges();
        }
        public void addParticipant(int userId, PlanParticipantDTO dto)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            User invitedUser = context.Users.Find(dto.userId);
            if (invitedUser == null) throw new Exception("No user found!");
            Plan plan = context.Plans.Find(dto.planId);
            if (plan == null) throw new Exception("No plan found!");

            if (!plan.participants.ContainsKey(user)) throw new Exception("Not included in this plan!");
            if (!plan.participants[user].Item1) throw new Exception("Accept invitation before adding users!");
            if (!plan.participants[user].Item2) throw new Exception("You need to be an admin to add users!");
            plan.addParticipant(invitedUser, false, dto.admin);
            context.SaveChanges();
        }
        public void removeParticipant(int userId, PlanParticipantDTO dto)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            User removedUser = context.Users.Find(dto.userId);
            if (removedUser == null) throw new Exception("No user found!");
            Plan plan = context.Plans.Find(dto.planId);
            if (plan == null) throw new Exception("No plan found!");

            if (user != plan.administrator)
            {
                if (!plan.participants.ContainsKey(user)) throw new Exception("Not included in this plan!");
                if (!plan.participants[user].Item1) throw new Exception("Accept invitation before removing users!");
                if (user != removedUser)
                {
                    if (!plan.participants[user].Item2) throw new Exception("You need to be an admin to remove users!");
                    if (!plan.participants.ContainsKey(removedUser)) throw new Exception("Can't remove user! Isn't a participant.");
                    if (plan.participants[removedUser].Item2) throw new Exception("You can't remove other admin!");
                }
            }
            else
            {
                if (!plan.participants.ContainsKey(removedUser)) throw new Exception("Can't remove user! Isn't a participant.");
                if (user == removedUser) throw new Exception("Can't remove yourselve as a creator!");
            }
            plan.removeParticipant(removedUser);
            context.SaveChanges();
        }
        public void acceptInvitation(int userId, PlanIdDTO dto)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            Plan plan = context.Plans.Find(dto.id);
            if (plan == null) throw new Exception("No plan found!");
            if (!plan.participants.ContainsKey(user)) throw new Exception("Not included in this plan!");
            if (plan.participants[user].Item1) throw new Exception("Already accepted!");
            plan.accept(user);
            context.SaveChanges();
        }
        public void rejectInvitation(int userId, PlanIdDTO dto)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            Plan plan = context.Plans.Find(dto.id);
            if (plan == null) throw new Exception("No plan found!");
            if (!plan.participants.ContainsKey(user)) throw new Exception("Not included in this plan!");
            if (plan.participants[user].Item1) throw new Exception("Already accepted!");
            plan.reject(user);
            context.SaveChanges();
        }
        public void setAdmin(int userId, PlanParticipantDTO dto)
        {
            User user = context.Users.Find(userId);
            if (user == null) throw new Exception("No user found!");
            User changedUser = context.Users.Find(dto.userId);
            if (changedUser == null) throw new Exception("No user found!");
            Plan plan = context.Plans.Find(dto.planId);
            if (plan == null) throw new Exception("No plan found!");

            if (!plan.participants.ContainsKey(user)) throw new Exception("Not included in this plan!");
            if (!plan.participants.ContainsKey(changedUser)) throw new Exception("This user not included in this plan!");

            if(plan.participants[changedUser].Item2==dto.admin) throw new Exception("This doesn't change the status!");

            if (user == plan.administrator)
            {
                if (user == changedUser) throw new Exception("Creator can't downgrade himself!");
            }
            else
            {
                if (user != changedUser) throw new Exception("Can't downgrade other admin!");
            }
            plan.setAdmin(changedUser, dto.admin);
            context.SaveChanges();
        }
    }
}
