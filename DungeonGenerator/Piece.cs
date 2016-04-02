using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
	public class Piece
	{
		public bool? ConnectBottom = null;
		public bool? ConnectTop = null;
		public bool? ConnectLeft = null;
		public bool? ConnectRight = null;

		public Piece()
		{
			
		}

		public Piece(bool b, bool t, bool l, bool r)
		{

			ConnectBottom = b;
			ConnectTop = t;
			ConnectLeft = l;
			ConnectRight = r;
		}
		
		public bool Equals(Piece p)
		{
			if (this.ConnectBottom == p.ConnectBottom && this.ConnectLeft == p.ConnectLeft && this.ConnectTop == p.ConnectTop && this.ConnectRight == p.ConnectRight)
				return true;
			else
				return false;
		}
	}
}
