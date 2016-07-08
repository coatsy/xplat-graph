﻿using System.Collections.Generic;

namespace PropertyManager.Models
{
    public class GroupModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public List<string> GroupTypes { get; set; }

        public string Mail { get; set; }

        public bool MailEnabled { get; set; }

        public string MailNickname { get; set; }

        public List<string> ProxyAddresses { get; set; }

        public bool SecurityEnabled { get; set; }

        public string Visibility { get; set; }

        public static GroupModel CreateUnified(string displayName, string description,
            string mailNickname)
        {
            return new GroupModel
            {
                DisplayName = displayName,
                Description = description,
                MailNickname = mailNickname,
                MailEnabled = true,
                SecurityEnabled = false,
                GroupTypes = new List<string> {"Unified"}
            };
        }
    }
}
