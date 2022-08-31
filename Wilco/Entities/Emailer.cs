using System.Net.Mail;

namespace Wilco;

public class Emailer
{
	public bool FormatAsHTML { get; set; } = false;
	public string From { get; set; } = "noreply@wilco.org";
	public string MailServer { get; } = "relay.wilco.org";
	public string Message { get; set; } = "Too lazy to write a message";
	public IEnumerable<string> Recipients { get; set; } = new[] { "sgietz@wilco.org" };
	public string Subject { get; }

	public Emailer(string subject) => Subject = subject;

	public Emailer(string subject, string body) => (Subject, Message) = (subject, body);

	public Emailer(string subject, string body, IEnumerable<string> recipients) => (Subject, Message, Recipients) = (subject, body, recipients);

	public Emailer(string subject, string body, IEnumerable<string> recipients, string mailServer) =>
		(Subject, Message, Recipients, MailServer) = (subject, body, recipients, mailServer);
	
	public void Send()
	{
		using var smtp = new SmtpClient(MailServer);
		using var mm = new MailMessage();
		mm.Body = Message;
		mm.From = new MailAddress(From);
		mm.Subject = Subject;
		mm.IsBodyHtml = FormatAsHTML;

		Recipients.ToList().ForEach(r => mm.To.Add(r));

		smtp.Send(mm);
	}

	#region Static Methods

	public static void Send(string subject) => Send(subject, "Too lazy for a message");

	public static void Send(string subject, string body) => Send(subject, body, new List<string> { "sgietz@wilco.org" });

	public static void Send(string subject, string body, List<string> recipients) => Send(subject, body, recipients, "relay.wilco.org");

	public static async void Send(string subject, string body, List<string> recipients, string mailServer)
	{
		using var smtp = new SmtpClient(mailServer);
		using var mm = new MailMessage();
		mm.Body = body;
		mm.From = new MailAddress("noreply@wilco.org");
		mm.IsBodyHtml = true;
		mm.Subject = subject;
		mm.To.Add("noreply@wilco.org");

		recipients.ForEach(r => mm.Bcc.Add(r));

		await Task.Run(() => smtp.Send(mm));
	}

	#endregion
}