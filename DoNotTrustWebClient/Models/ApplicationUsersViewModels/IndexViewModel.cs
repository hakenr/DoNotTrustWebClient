using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoNotTrustWebClient.Models.ApplicationUsersViewModels
{
    public class IndexViewModel
    {
		public IEnumerable<SelectListItem> Departments { get; set; }

		public int? DepartmentId { get; set; }

		public IEnumerable<ApplicationUser> Users { get; set; }
	}
}
