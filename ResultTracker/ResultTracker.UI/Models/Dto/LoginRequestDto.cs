﻿using System.ComponentModel.DataAnnotations;

namespace ResultTracker.UI.Models.Dto
{
	public class LoginRequestDto
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Username { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
