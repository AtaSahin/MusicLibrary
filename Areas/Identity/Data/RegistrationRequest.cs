using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
namespace MusicLibraryApp.Areas.Identity.Data
{
    public class RegistrationRequest : IdentityUser
    {
        public DateTime RequestDate { get; set; }
    }
}
