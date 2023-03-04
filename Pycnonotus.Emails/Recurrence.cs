using System.Collections.Generic;

namespace Pycnonotus.Emails.Settings
{
	public class Recurrence
	{
		public string Frequency { get; set; } // DAILY, WEEKLY, MONTHLY, YEARLY
		public int Interval { get; set; } // Interval in the specified frequency (e.g. every 2 weeks)
		public int Count { get; set; } // Number of occurrences (optional)
		public List<string> ByDays { get; set; } // List of days of the week (e.g. MO,WE,FR for every Monday, Wednesday, and Friday)
	}
}
