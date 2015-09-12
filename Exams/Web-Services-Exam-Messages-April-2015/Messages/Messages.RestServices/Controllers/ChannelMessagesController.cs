using Microsoft.AspNet.Identity;

namespace Messages.RestServices.Controllers
{
    using Messages.Data;
    using Messages.Data.Models;
    using Messages.RestServices.BindingModels;
    using Messages.RestServices.UnitOfWork;
    using Messages.RestServices.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [RoutePrefix("api/channel-messages")]
    public class ChannelMessagesController : ApiController
    {
        private IUnitOfWork db;

        public ChannelMessagesController()
            :this(new UnitOfWork())
        {
            
        }

         public ChannelMessagesController(UnitOfWork context)
         {
             this.db = context;
         }

        [Route("{channelName}")]
        public IHttpActionResult GetByChannelName(string channelName)
        {
            var channel = db.Channels.All().FirstOrDefault(c => c.Name == channelName);

            if (channel == null)
            {
                return NotFound();
            }

            var messages = channel.ChannelMessages
                .OrderByDescending(cm => cm.DateSent)
                .Select(cm => new ChannelMessagesViewModel()
            {
                Id = cm.Id,
                Text = cm.Text,
                DateSent = cm.DateSent,
                Sender = cm.Sender == null ? null : cm.Sender.UserName
            });

            return this.Ok(messages);
        }

        [Route("{channelName}")]
        public IHttpActionResult GetByChannelName(string channelName, [FromUri] string limit)
        {
            var channel = db.Channels.All().FirstOrDefault(c => c.Name == channelName);

            if (channel == null)
            {
                return NotFound();
            }

            int messagesLimit = 0;

            try
            {
                messagesLimit = int.Parse(limit);
            }
            catch (FormatException)
            {
                return BadRequest();
            }

            if (messagesLimit < 1 || messagesLimit > 1000)
            {
                return BadRequest(); 
            }

            var messages = channel.ChannelMessages
                .OrderBy(cm => cm.Text)
                .ThenByDescending(cm => cm.DateSent)
                .Select(cm => new ChannelMessagesViewModel()
                {
                    Id = cm.Id,
                    Text = cm.Text,
                    DateSent = cm.DateSent,
                    Sender = cm.Sender == null ? null : cm.Sender.UserName
                })
                .Take(messagesLimit);

            return this.Ok(messages);
        }

        [Route("{channelName}")]
        public IHttpActionResult PostChannelMessage(string channelName, [FromBody] ChannelMessageBindingModel channelMessageModel)
        {
            var channel = db.Channels.All().FirstOrDefault(c => c.Name == channelName);
            var userId = User.Identity.GetUserId();
            User user = null;
            if (userId != null)
            {
                user = db.Users.Find(int.Parse(userId));
            }

            if (channel == null)
            {
                return NotFound();
            }

            if (channelMessageModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (user == null && userId != null)
            {
                return Unauthorized();
            }

            var channelMessage = new ChannelMessage()
            {
                Text = channelMessageModel.Text,
                DateSent = DateTime.Now,
                Sender = user
            };

            channel.ChannelMessages.Add(channelMessage);

            db.SaveChanges();

            if (user == null)
            {
                return this.Ok(new
                {
                    Id = channelMessage.Id,
                    Message = "Anonymous message sent to channel " + channel.Name + "."
                });
            }

            return this.Ok(new
            {
                Id = channelMessage.Id,
                Sender = user.UserName,
                Message = "Message sent to channel " + channel.Name + "."
            });
            
        }
    }
}
