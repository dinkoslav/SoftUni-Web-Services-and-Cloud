using Messages.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using System.Net;
using Messages.Tests.Models;
using System.Net.Http;

namespace Messages.Tests
{
    [TestClass]
    public class ChannelIntegrationTests
    {
        [TestMethod]
        public void DeleteChannel_DeleteExistingChannel_ShouldReturn200OK()
        {
            // Arrange -> prepare a few channels
            TestingEngine.CleanDatabase();
            var channelName = "deleteme";

            // Act -> create a few channels
            this.CreateChannelHttpPost(channelName);
            var deleteResponseMessage = this.DeleteChannelHttpDelete(channelName);

            Assert.AreEqual(HttpStatusCode.OK, deleteResponseMessage.StatusCode);
            Assert.AreEqual("", deleteResponseMessage.Content);
            Assert.AreEqual(0, TestingEngine.GetChannelsCountFromDb());

        }

        private HttpResponseMessage DeleteChannelHttpDelete(string channelName)
        {
            var httpResponse = TestingEngine.HttpClient.DeleteAsync("api/channels/" + channelName).Result;
            return httpResponse;
        }
        
        public static void CleanDatabase()
        {
            using (var dbContext = new MessagesDbContext())
            {
                dbContext.ChannelMessages.Delete();
                dbContext.UserMessages.Delete();
                dbContext.Users.Delete();
                dbContext.Channels.Delete();
                dbContext.SaveChanges();
            }
        }

        private HttpResponseMessage CreateChannelHttpPost(string channelName)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("name", channelName)
            });
            var httpPostResponse = TestingEngine.HttpClient.PostAsync(
                "/api/channels", postContent).Result;
            return httpPostResponse;
        }
    }
}
