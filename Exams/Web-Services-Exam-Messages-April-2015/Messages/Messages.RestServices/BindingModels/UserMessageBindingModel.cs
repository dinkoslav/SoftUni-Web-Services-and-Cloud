using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Messages.RestServices.BindingModels
{
    public class UserMessageBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Text { get; set; }
    }
}