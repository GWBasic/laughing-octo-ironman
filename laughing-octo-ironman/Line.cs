using System;

namespace laughingoctoironman
{
	public class Line
	{
		public Line (int number, string contents, Random random)
		{
			this.number = number;
			this.contents = contents;

			var id = new byte[32];
			random.NextBytes(id);
			this.id = Convert.ToBase64String(id);
		}

		/// <summary>
		/// The line number
		/// </summary>
		public int Number 
		{
			get { return number; }
		}
		private readonly int number;

		/// <summary>
		/// The actual linke
		/// </summary>
		public string Contents 
		{
			get { return contents; }
		}
		private readonly string contents;

		public string Id 
		{
			get { return id; }
		}
		private readonly string id;
	}
}

