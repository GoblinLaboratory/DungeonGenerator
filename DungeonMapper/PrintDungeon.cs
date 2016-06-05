using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGenerator;

namespace DungeonMapper
{
	class PrintDungeon
	{
		public static void Main()
		{
			while(true)
			{
				Piece[][] Map = CreateDungeon.DungeonSetup(20, 10);
				Map = CreateDungeon.DungeonFill(Map);
				CreateDungeon.PrintDung(Map);
				Console.Read();
				Console.Clear();
			}
		}
	}
}
