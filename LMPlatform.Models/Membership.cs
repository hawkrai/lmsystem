using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;

namespace LMPlatform.Models
{
	public class Membership : ModelBase
	{
		public Membership()
		{
			Roles = new List<Role>();
			OAuthMemberships = new List<OAuthMembership>();
		}

		public DateTime? CreateDate
		{
			get;
			set;
		}

		[StringLength(128)]
		public string ConfirmationToken
		{
			get;
			set;
		}

		public bool? IsConfirmed
		{
			get;
			set;
		}

		public DateTime? LastPasswordFailureDate
		{
			get;
			set;
		}

		public int PasswordFailuresSinceLastSuccess
		{
			get;
			set;
		}

		[Required, StringLength(128)]
		public string Password
		{
			get;
			set;
		}

		public DateTime? PasswordChangedDate
		{
			get;
			set;
		}

		[Required, StringLength(128)]
		public string PasswordSalt
		{
			get;
			set;
		}

		[StringLength(128)]
		public string PasswordVerificationToken
		{
			get;
			set;
		}

		public DateTime? PasswordVerificationTokenExpirationDate
		{
			get;
			set;
		}

		public ICollection<Role> Roles
		{
			get;
			set;
		}

		public User User
		{
			get;
			set;
		}

		[ForeignKey("UserId")]
		public ICollection<OAuthMembership> OAuthMemberships
		{
			get;
			set;
		}
	}
}