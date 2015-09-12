namespace Messages.RestServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Messages.Data;
    using Messages.Data.Models;
    using Messages.RestServices.ViewModels;
    using Messages.RestServices.UnitOfWork;

    public class ChannelsController : ApiController
    {
        private IUnitOfWork db;

        public ChannelsController()
            :this(new UnitOfWork())
        {
            
        }

        public ChannelsController(UnitOfWork context)
         {
             this.db = context;
         }

        // GET: api/Channels
        [HttpGet]
        public IQueryable<ChannelViewModel> GetChannels()
        {
            return db.Channels
                .All()
                .OrderBy(c => c.Name)
                .Select(c => new ChannelViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                });
        }

        // GET: api/Channels/{id}
        [HttpGet]
        [ResponseType(typeof(ChannelViewModel))]
        public IHttpActionResult GetChannel(int id)
        {
            Channel channel = db.Channels.Find(id);
            if (channel == null)
            {
                return NotFound();
            }

            return Ok(new ChannelViewModel()
            {
                Id = channel.Id,
                Name = channel.Name
            });
        }

        // PUT: api/Channels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChannel(int id, Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (channel == null)
            {
                return BadRequest();
            }

            var dbChannel = db.Channels.Find(id);

            if (dbChannel == null)
            {
                return NotFound();
            }

            if (db.Channels.All().Any(c => c.Name == channel.Name && channel.Id != id))
            {
                return Conflict();
            }

            dbChannel.Name = channel.Name;
            db.SaveChanges();

            return this.Ok(new
            {
                Message = "Channel #" + id + " edited successfully."
            });
        }

        // POST: api/Channels
        [ResponseType(typeof(ChannelViewModel))]
        public IHttpActionResult PostChannel(Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (channel == null)
            {
                return BadRequest();
            }

            if (db.Channels.All().Any(c => c.Name == channel.Name))
            {
                return Conflict();
            }

            db.Channels.Add(channel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", 
                new { id = channel.Id }, 
                new ChannelViewModel()
                {
                    Id = channel.Id,
                    Name = channel.Name
                });
        }

        // DELETE: api/Channels/{id}
        [ResponseType(typeof(Channel))]
        public IHttpActionResult DeleteChannel(int id)
        {
            Channel channel = db.Channels.Find(id);
            if (channel == null)
            {
                return NotFound();
            }

            if (channel.ChannelMessages.Any())
            {
                return ResponseMessage(
                    Request.CreateResponse(
                    HttpStatusCode.Conflict,
                    new { Message = "Cannot delete channel #" + id + " because it is not empty."})
                    );
            }

            db.Channels.Remove(channel);
            db.SaveChanges();

            return Ok(new 
            {
                Message = "Channel #" + id + " deleted."
            });
        }
    }
}