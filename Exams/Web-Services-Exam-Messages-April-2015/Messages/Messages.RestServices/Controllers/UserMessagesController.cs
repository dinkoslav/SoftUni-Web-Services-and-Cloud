namespace Messages.RestServices.Controllers
{
    using Messages.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Messages.RestServices.ViewModels;
    using Messages.RestServices.BindingModels;
    using Messages.Data.Models;
    using Messages.RestServices.UnitOfWork;

    [RoutePrefix("api/user")]
    public class UserMessagesController : ApiController
    {
        private IUnitOfWork db;

        public UserMessagesController()
            :this(new UnitOfWork())
        {
            
        }

        public UserMessagesController(UnitOfWork context)
         {
             this.db = context;
         }

        [Route("personal-messages")]
        public IHttpActionResult GetPersonalMessages()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = db.Users.Find(int.Parse(userId));

            if (user == null)
            {
                return Unauthorized();
            }

            var userMessages = db.UserMessages
                .All()
                .Where(um => um.Reciever.Id == user.Id)
                .OrderByDescending(um => um.DateSent)
                .Select(um => new UserMessageViewModel()
                {
                    Id = um.Id,
                    Text = um.Text,
                    DateSent = um.DateSent,
                    Sender = um.Sender == null ? null : um.Sender.UserName
                });

            return this.Ok(userMessages);
        }

        [Route("personal-messages")]
        public IHttpActionResult PostPersonalMessages([FromBody] UserMessageBindingModel userMessageModel)
        {
            if (!ModelState.IsValid || userMessageModel == null || string.IsNullOrEmpty(userMessageModel.Text))
            {
                return BadRequest();
            }

            var recipient = db.Users.All().FirstOrDefault(u => u.UserName == userMessageModel.UserName);

            if (recipient == null)
            {
                return NotFound();
            }

            var userId = User.Identity.GetUserId();
            User user = null;

            if (userId != null)
            {
                user = db.Users.All().FirstOrDefault(u => u.Id == userId);
            }

            if (userId != null && user == null)
            {
                return Unauthorized();
            }

            var userMessage = new UserMessage()
            {
                Text = userMessageModel.Text,
                DateSent = DateTime.Now,
                Reciever = recipient,
                Sender = user
            };

            db.UserMessages.Add(userMessage);
            db.SaveChanges();

            if (user == null)
            {
                return this.Ok(new
                {
                    Id = userMessage.Id,
                    Message = "Anonymous message sent successfully to user " + recipient.UserName + "."
                });
            }

            return this.Ok(new
            {
                Id = userMessage.Id,
                Sender = user.UserName,
                Message = "Message sent to user  " + recipient.UserName + "."
            });
        }
    }
}
