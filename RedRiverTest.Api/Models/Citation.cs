namespace RedRiverTest.Api.Models
{
	public class Citation
	{
		public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty; // link to logged in user
	}
}
