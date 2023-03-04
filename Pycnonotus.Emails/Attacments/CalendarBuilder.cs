using System;
using System.Text;

namespace Pycnonotus.Emails.Settings.Attacments;

public class CalendarBuilder
{
	

	public static string GetICalendar()
	{
		// Define the event information
		string eventName = "My Event";
		DateTime eventStart = DateTime.Now.AddHours(3);
		DateTime eventEnd = eventStart.AddHours(6);
		string location = "My Location";
		string description = "My Description";

		// Create the iCalendar file
		StringBuilder icalStringBuilder = new StringBuilder();
		icalStringBuilder.AppendLine("BEGIN:VCALENDAR");
		icalStringBuilder.AppendLine("VERSION:2.0");
		icalStringBuilder.AppendLine("PRODID:-//My Company//My Product//EN");

		// Create the event
		icalStringBuilder.AppendLine("BEGIN:VEVENT");
		icalStringBuilder.AppendLine($"UID:{Guid.NewGuid()}");
		icalStringBuilder.AppendLine($"DTSTAMP:{DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ")}");
		icalStringBuilder.AppendLine($"DTSTART:{eventStart.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")}");
		icalStringBuilder.AppendLine($"DTEND:{eventEnd.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")}");
		icalStringBuilder.AppendLine($"SUMMARY:{eventName}");
		icalStringBuilder.AppendLine($"LOCATION:{location}");
		icalStringBuilder.AppendLine($"DESCRIPTION:{description}");
		icalStringBuilder.AppendLine("END:VEVENT");

		// End the iCalendar file
		icalStringBuilder.AppendLine("END:VCALENDAR");

		// Return the iCalendar file as a string
		return icalStringBuilder.ToString();
	}
}
