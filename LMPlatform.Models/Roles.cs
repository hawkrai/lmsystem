using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;

namespace LMPlatform.Models
{
	public class Role : ModelBase
	{
		public Role()
		{
			Members = new List<Membership>();
		}

		[StringLength(256)]
		public string RoleName
		{
			get;
			set;
		}

		[StringLength(256)]
		public string RoleDisplayName
		{
			get;
			set;
		}

		public ICollection<Membership> Members
		{
			get;
			set;
		}
	}
}