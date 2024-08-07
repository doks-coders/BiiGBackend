﻿using Microsoft.AspNetCore.Identity;

namespace BiiGBackend.Models.Entities.Identity
{
	public class AppRole : IdentityRole<Guid>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
