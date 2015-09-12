﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messages.RestServices.ViewModels
{
    public class UserMessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }
    }
}