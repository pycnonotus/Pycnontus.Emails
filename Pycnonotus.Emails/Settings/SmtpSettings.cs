﻿

namespace Pycnonotus.Emails.Settings
{

	public class SmtpSettings : ISmtpSettings
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}

}
