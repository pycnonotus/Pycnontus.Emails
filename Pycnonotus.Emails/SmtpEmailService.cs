using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace Pycnonotus.Emails.Settings
{
	public class SmtpEmailService
	{
		private readonly ISmtpSettings _smtpSettings;
		private readonly string _from;

		public SmtpEmailService(SmtpMailSettings smtpMailSettings)
		{
			_smtpSettings = smtpMailSettings.SmtpSettings;
			_from = smtpMailSettings.From;
		}


		public void SendMeetingInvitation(string to, string subject, string body, DateTime start, DateTime end,
			string location, Recurrence recurrence, List<Attendee> attendees, bool requireOutlookConfirmation = false)
		{
			// Construct iCalendar file format
			var iCal = new StringBuilder();
			iCal.AppendLine("BEGIN:VCALENDAR");
			iCal.AppendLine("PRODID:-//Microsoft Corporation//Outlook 16.0 MIMEDIR//EN");
			iCal.AppendLine("VERSION:2.0");
			iCal.AppendLine("METHOD:REQUEST");
			iCal.AppendLine("BEGIN:VEVENT");
			iCal.AppendLine($"UID:{Guid.NewGuid():D}");
			iCal.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmss}");

			iCal.AppendLine($"DTSTART:{start:yyyyMMddTHHmmssZ}");
			iCal.AppendLine($"DTEND:{end:yyyyMMddTHHmmssZ}");
			iCal.AppendLine($"LOCATION:{location}");
			iCal.AppendLine($"DESCRIPTION:{body}");
			iCal.AppendLine($"SUMMARY:{subject}");
			iCal.AppendLine($"ORGANIZER:MAILTO:{_from}");

			// Add attendees
			foreach (var attendee in attendees)
			{
				iCal.AppendLine($"ATTENDEE;CN=\"{attendee.DisplayName}\":mailto:{attendee.Email}");
				iCal.AppendLine($"X-MICROSOFT-CDO-BUSYSTATUS:TENTATIVE");
				if (!string.IsNullOrEmpty(attendee.ResponseStatus))
				{
					iCal.AppendLine(
						$"ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT={attendee.ResponseStatus.ToUpperInvariant()};CN=\"{attendee.DisplayName}\":MAILTO:{attendee.Email}");
				}
				else
				{
					iCal.AppendLine(
						$"ATTENDEE;ROLE=REQ-PARTICIPANT;CN=\"{attendee.DisplayName}\":MAILTO:{attendee.Email}");
				}
			}

			// Add recurrence rule
			iCal.AppendLine($"RRULE:FREQ={recurrence.Frequency};INTERVAL={recurrence.Interval}");
			if (recurrence.Count > 0)
			{
				iCal.AppendLine($"COUNT={recurrence.Count}");
			}

			if (recurrence.ByDays != null && recurrence.ByDays.Any())
			{
				iCal.AppendLine($"BYDAY={string.Join(",", recurrence.ByDays)}");
			}

			iCal.AppendLine("END:VEVENT");
			iCal.AppendLine("END:VCALENDAR");

			// Create email message with iCalendar file attached
			var message = new MailMessage(_from, to, subject, body);

			// var attachment = CreateAttachment(iCal.ToString(), "meeting.ics");
			// message.Attachments.Add(attachment);

			// Set message headers for Outlook compatibility
			message.Headers.Add("Content-class", "urn:content-classes:calendarmessage");
			message.Headers.Add("Disposition-Notification-To",_from);
			message.Headers.Add("X-MS-OLK-REQSTARTTIME", $"{start.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}");
			message.Headers.Add("X-MS-OLK-REQENDTIME", $"{end.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}");
			message.Headers.Add("X-MS-OLK-CONFTYPE", "1");
			message.Headers.Add("X-MS-OLK-SENDER", $"mailto:{_from}");
			message.Headers.Add("X-MS-OLK-ALLDAYEVENT", "FALSE");
			message.Headers.Add("X-MS-OLK-APPTSEQTIME", $"{DateTime.UtcNow.Ticks:x}");
			message.Headers.Add("X-MS-OLK-RECURRING", recurrence != null ? "TRUE" : "FALSE");
			message.Headers.Add("X-MS-OLK-ORIGINALSTART", $"{start.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}");
			message.Headers.Add("X-MS-OLK-OWNERCRITICALCHANGE", $"{DateTime.UtcNow.Ticks:x}");
			message.Headers.Add("Content-class", "urn:content-classes:calendarmessage");
			System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar; method=REQUEST");

			AlternateView avCal = AlternateView.CreateAlternateViewFromString(iCal.ToString(), contype);
			message.AlternateViews.Add(avCal);
			if (requireOutlookConfirmation)
			{
				message.Headers.Add("X-MS-OLK-SENDOUTLOOKREQUEST", "TRUE");
			}

			var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
			{
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
				EnableSsl = true // Use SSL encryption for added security
			};
			smtpClient.Send(message);

		}

		private static Attachment CreateAttachment(string content, string fileName)
		{
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
			var attachment = new Attachment(stream, fileName, "text/calendar; method=REQUEST");
			attachment.ContentDisposition.Inline = true;
			attachment.ContentType = new System.Net.Mime.ContentType("text/calendar; method=REQUEST");
			attachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
			return attachment;
		}
	}
}
