using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Data.Entities
{
    public class ApplicationRole : IdentityRole<long>
    {
        public DateTime CreatedDate { get; set; }
    }
}
