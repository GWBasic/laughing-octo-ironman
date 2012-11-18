using System;

namespace laughingoctoironman
{
	public class Line
	{
		public Line (int number, string contents)
		{
			this.number = number;
			this.contents = contents;
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
	}
}

