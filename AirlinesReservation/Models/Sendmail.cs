using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlinesReservation.Models
{
    public class ContactFormModel
    {

        public string Name { get; set; }


        public string Subject { get; set; }


        public string Email { get; set; }


        [AllowHtml]
        public string Body { get; set; }

    }
}