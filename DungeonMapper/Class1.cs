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
		public static void PrintDung(Piece[][] Map)
		{
			Piece Edge = new Piece(false, false, false, false);
			
			for (int i = 0; i < Map.Length; i++)
			{
				for (int j = 0; j < Map[i].Length; j++)
				{
					if (Map[i][j] != null && Edge.Equals(Map[i][j]))
						if (j == Map[i].Length - 1)
							Console.WriteLine("e ");
						else
							Console.Write("e ");
					else if (Map[i][j].GetType() == Edge.GetType() && !Map[i][j].Equals(new Piece()))
					{
						Console.Write("f ");
					}
					else
						Console.Write("x ");
				}
			}
		}
		public static void Main()
		{
			Piece[][] Map = CreateDungeon.DungeonSetup(5, 5);
			PrintDung(Map);
			Map = DungeonGenerator.CreateDungeon.DungeonFill(Map);
			PrintDung(Map);

			Console.WriteLine();
		}
	}
}
