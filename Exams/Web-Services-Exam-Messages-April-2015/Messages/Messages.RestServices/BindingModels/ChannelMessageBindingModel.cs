﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Messages.RestServices.BindingModels
{
    public class ChannelMessageBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}