using System;
using Pycnonotus.Emails.Settings;
using Xunit;

namespace Pycnonotus.Emails.UnitTests
{
	public class SmtpSettingsUnitTest
	{
		[Fact]
		public void CanSetAndGetHost()
		{
			// Arrange
			var smtpSettings = new SmtpSettings();

			// Act
			smtpSettings.Host = "smtp.example.com";

			// Assert
			Assert.Equal("smtp.example.com", smtpSettings.Host);
		}

		[Fact]
		public void CanSetAndGetPort()
		{
			// Arrange
			var smtpSettings = new SmtpSettings();

			// Act
			smtpSettings.Port = 587;

			// Assert
			Assert.Equal(587, smtpSettings.Port);
		}

		[Fact]
		public void CanSetAndGetUserName()
		{
			// Arrange
			var smtpSettings = new SmtpSettings();

			// Act
			smtpSettings.UserName = "user";

			// Assert
			Assert.Equal("user", smtpSettings.UserName);
		}

		[Fact]
		public void CanSetAndGetPassword()
		{
			// Arrange
			var smtpSettings = new SmtpSettings();

			// Act
			smtpSettings.Password = "password";

			// Assert
			Assert.Equal("password", smtpSettings.Password);
		}
	}
}
