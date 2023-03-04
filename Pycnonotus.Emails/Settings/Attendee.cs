namespace Pycnonotus.Emails.Settings
{
	public class Attendee
	{
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public string ResponseStatus { get; set; } // Accepted, Declined, Tentative, None
	}
}
