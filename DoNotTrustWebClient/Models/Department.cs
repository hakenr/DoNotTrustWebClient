using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotTrustWebClient.Models
{
    public class Department
    {
		public int DepartmentId { get; set; }

		public Company Company { get; set; }

		public string Name { get; set; }
	}
}
