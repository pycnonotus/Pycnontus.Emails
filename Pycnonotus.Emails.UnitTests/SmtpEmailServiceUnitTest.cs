using System;
using System.Collections.Generic;
using System.Net;
using Pycnonotus.Emails.Settings;
using Xunit;

namespace Pycnonotus.Emails.UnitTests
{
	public class SmtpEmailServiceUnitTest
	{
		private readonly ISmtpSettings _smtpSettings;
		private SmtpMailSettings _smtpMailSettings;

		public SmtpEmailServiceUnitTest()
		{
			_smtpSettings = new SmtpSettings
			{
				Host = "antonweb.co.il",
				Port = 587,
				UserName = "root",
				Password = "e33F6cwE4j7"
			};
			_smtpMailSettings = new SmtpMailSettings()
			{
				SmtpSettings = _smtpSettings,
				From = "meetings@antonweb.co.il"
			};
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			

		}

	

		[Fact]
		public void SendMeetingInvitation_ShouldSendMeetingInvitation()
		{
			// Arrange
			var emailSender = new SmtpEmailService(_smtpMailSettings);
			var to = "anton.golov@ideo-digital.com";
			// var to = "anton.golo.fh@gmail.com";
			var subject = "בדיקה אחרונה וזהו ו";
			var body = "TEst";
			var start = DateTime.Now.AddMonths(1).Date.AddHours(2);
			var end = start.AddHours(1);
			var location = "Conference Room";

			// Act
			var recurnece = new Recurrence()
			{
				Count = 6,
				Frequency = "DAILY",
				Interval = 4,
			};

			var attends = new List<Attendee>()
			{
				new Attendee()
				{
					Email = to,
					DisplayName = "Anton Go",
					ResponseStatus = "ACCEPT"
				}
			};
			
			emailSender.SendMeetingInvitation(to, subject, body, start, end, location,recurnece,attends,true);

			// Assert
			// No exception thrown means meeting invitation was sent successfully
		}
	}
}
