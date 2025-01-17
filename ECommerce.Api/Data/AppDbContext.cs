﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Data
{
    namespace ECommerce.Api.Data
    {
        public class AppDbContext : IdentityDbContext<AppUser>
        {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {

            }
        }
    }
}
