using System;
using System.Collections.Generic;
using DQF.Common.Account;
using DQF.Domain.Aggregates.User;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace DQF.Views
{
    public class UserView
    {
        [DocumentId, BsonId]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
        public UserRoleEnum Role { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> AdditionalEmails { get; set; }

        public List<string> AllEmails
        {
            get {return new List<string>(AdditionalEmails){Email}; }
        }

        public UserView()
        {
            AdditionalEmails = new List<string>();
        }
    }
}