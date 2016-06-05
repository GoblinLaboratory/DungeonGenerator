using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DungeonGenerator
{
	public class CreateDungeon
	{
		public static string PrintPiece(Piece p)
		{
			string sPiece = "(";
			if (p.ConnectBottom == true)
				sPiece += "true, ";
			else if (p.ConnectBottom == false)
				sPiece += "false, ";
			else
				sPiece += "null, ";
			if (p.ConnectTop == true)
				sPiece += "true, ";
			else if (p.ConnectTop == false)
				sPiece += "false, ";
			else
				sPiece += "null, ";
			if (p.ConnectLeft == true)
				sPiece += "true, ";
			else if (p.ConnectLeft == false)
				sPiece += "false, ";
			else
				sPiece += "null, ";
			if (p.ConnectRight == true)
				sPiece += "true)";
			else if (p.ConnectRight == false)
				sPiece += "false) ";
			else
				sPiece += "null)";
			return sPiece;
		}

		enum Moves { Up, Down, Left, Right, None };
		
		static Piece[] AllPieces = new Piece[15]
			{
            new Piece(true, false, false, false),  //b
            new Piece(false, true, false, false),  //t
            new Piece(false, false, true, false),  //l
            new Piece(false, false, false, true),  //r
            new Piece(true, false, true, false),  //bl
            new Piece(false, true, true, false),  //tl
            new Piece(false, false, true, true),  //lr
            new Piece(true, true, false, false),  //bt
            new Piece(false, true, false, true),  //tr
            new Piece(true, false, false, true),  //br
            new Piece(true, true, false, true),  //btr
            new Piece(true, true, true, false),  //btl
            new Piece(false, true, true, true),  //tlr
            new Piece(true, false, true, true),  //blr
            new Piece(true, true, true, true)   //btlr
            };

		static Piece[] NoEndPieces = new Piece[11]
		{
			new Piece(true, false, true, false),  //bl
            new Piece(false, true, true, false),  //tl
            new Piece(false, false, true, true),  //lr
            new Piece(true, true, false, false),  //bt
            new Piece(false, true, false, true),  //tr
            new Piece(true, false, false, true),  //br
            new Piece(true, true, false, true),  //btr
            new Piece(true, true, true, false),  //btl
            new Piece(false, true, true, true),  //tlr
            new Piece(true, false, true, true),  //blr
            new Piece(true, true, true, true)   //btlr
		};
		public static Piece[][] DungeonSetup(int W, int H)
		{
			Piece[][] Width = new Piece[W + 2][];
			Piece Edge = new Piece(false, false, false, false);

			for (int i = 0; i < Width.Length; i++)
			{
				Width[i] = new Piece[H + 2];
			}

			for (int i = 0; i < Width.Length; i++)
			{
				if (i == 0)
				{
					for (int j = 0; j < Width[i].Length; j++)
					{
						Width[i][j] = Edge;
					}
				}

				else if (i == Width.Length - 1)
				{
					for (int j = 0; j < Width[i].Length; j++)
					{
						Width[i][j] = Edge;
					}
				}

				else
				{
					Width[i][0] = Edge;
					for (int k = 1; k < Width[i].Length - 1; k++)
					{
						Width[i][k] = new Piece();
					}
					Width[i][Width[i].Length - 1] = Edge;
				}
			}

			return Width;
		}

		public static Piece[][] DungeonFill(Piece[][] m)
		{
			int PieceCount = 0;
			int MinDungSize = (m.Length - 2) * (m[0].Length - 2) / 2;
			while (PieceCount < MinDungSize)
			{
				Random random = new Random();
				Piece NullPiece = new Piece();
				Piece pPiece = m[0][0];
				int w = random.Next(1, m.Length - 2);
				int h = random.Next(1, m[0].Length - 2);

				bool placing = true;
				bool NoMoves = false;
				bool Looping = false;
				int LoopCount = 0;


				while (placing)
				{
					Piece Above = m[w][h - 1];
					Piece Below = m[w][h + 1];
					Piece Left = m[w - 1][h];
					Piece Right = m[w + 1][h];
					Piece cPiece = m[w][h];


					if (pPiece == cPiece)
						++LoopCount;
					if (LoopCount > 10)
						Looping = true;

					//placing a piece
					if (cPiece.Equals(NullPiece) && NoMoves == false)
					{
						List<Piece> select;
						if (PieceCount < MinDungSize && !Looping) //cant pick an end piece if the map isnt half filled
							select = NoEndPieces.ToList();
						else
							select = AllPieces.ToList();

						#region LINQ
						if (Below.ConnectTop == true)
						{
							select = select
							.Where(p => p.ConnectBottom == true)
							.ToList();
						}

						if (Above.ConnectBottom == true)
						{
							select = select
							.Where(p => p.ConnectTop == true)
							.Select(p => p).ToList();
						}

						if (Left.ConnectRight == true)
						{
							select = select
							.Where(p => p.ConnectLeft == true)
							.Select(p => p).ToList();
						}

						if (Right.ConnectLeft == true)
						{
							select = select
							.Where(p => p.ConnectRight == true)
							.Select(p => p).ToList();
						}

						if (Below.ConnectTop == false)
						{
							select = select
							.Where(p => p.ConnectBottom == false)
							.Select(p => p).ToList();
						}

						if (Above.ConnectBottom == false)
						{
							select = select
							.Where(p => p.ConnectTop == false)
							.Select(p => p).ToList();
						}

						if (Left.ConnectRight == false)
						{
							select = select
							.Where(p => p.ConnectLeft == false)
							.Select(p => p).ToList();
						}

						if (Right.ConnectLeft == false)
						{
							select = select
							.Where(p => p.ConnectRight == false)
							.Select(p => p).ToList();
						}

						#endregion LINQ
						if (select.Count != 0)
						{
							Piece tPiece = select[random.Next(0, select.Count - 1)];
							cPiece.ConnectBottom = tPiece.ConnectBottom;
							cPiece.ConnectTop = tPiece.ConnectTop;
							cPiece.ConnectLeft = tPiece.ConnectLeft;
							cPiece.ConnectRight = tPiece.ConnectRight;
							++PieceCount;
							Looping = false;
							LoopCount = 0;
							//PrintDung(m, w, h);
						}
						else
							NoMoves = true;
					}
					//moving to a new null block to place
					else if (!NoMoves)
					{
						if (Left.Equals(NullPiece) && cPiece.ConnectLeft == true)
							w -= 1;

						else if (Right.Equals(NullPiece) && cPiece.ConnectRight == true)
							w += 1;

						else if (Above.Equals(NullPiece) && cPiece.ConnectTop == true)
							h -= 1;

						else if (Below.Equals(NullPiece) && cPiece.ConnectBottom == true)
							h += 1;

						else
							NoMoves = true;

						//PrintDung(m, w, h);
					}

					//move to a block than can place something
					else if (NoMoves)
					{
						w = 1;
						h = 1;
						int DungSize = (m.Length - 2) * (m[0].Length - 2);

						for (int i = 0; i < DungSize; i++)
						{
							//PrintDung(m, w, h);

							Above = m[w][h - 1];
							Below = m[w][h + 1];
							Left = m[w - 1][h];
							Right = m[w + 1][h];
							cPiece = m[w][h];

							if (cPiece.Equals(NullPiece) && ((Above.ConnectBottom == true) || (Below.ConnectTop == true) || (Left.ConnectRight == true) || (Right.ConnectLeft == true)))
							{
								NoMoves = false;
								break;
							}
							else
							{
								if (w == m.Length - 2)
								{
									w = 1;
									++h;
								}
								else
									++w;
							}
							if (i == DungSize - 1)
								placing = false;
						}
					}
					pPiece = cPiece;
				}
			}
			return m;
			
		}

	public static void PrintDung(Piece[][] Map, int w, int h)
		{
			Piece Edge = new Piece(false, false, false, false);

			for (int i = 0; i < Map[0].Length; i++)
			{
				for (int j = 0; j < Map.Length; j++)
				{
					Console.ResetColor();
					if (j == w && i == h)
					{
						Console.ForegroundColor = ConsoleColor.Black;
						Console.BackgroundColor = ConsoleColor.Red;
					}
					if (Map[j][i] != null && Edge.Equals(Map[j][i]))
						if (j == Map.Length - 1)
							Console.WriteLine("e");
						else
							Console.Write("e");
					else if (!Map[j][i].Equals(new Piece()))
					{
						Piece p = Map[j][i];
						if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("┼");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("┴");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("├");
						else if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("┤");
						else if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("┬");
						else if (p.ConnectBottom == false && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("└");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("┘");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("─");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("┌");
						else if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == false)
							Console.Write("┐");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("│");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == false && p.ConnectTop == false)
							Console.Write("^");
						else if (p.ConnectBottom == false && p.ConnectLeft == false && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("v");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == false)
							Console.Write("]");
						else if (p.ConnectBottom == false && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("[");
					}
					else
						Console.Write(".");
				}
			}
		}

		public static void PrintDung(Piece[][] Map)
		{
			Piece Edge = new Piece(false, false, false, false);

			for (int i = 0; i < Map[0].Length; i++)
			{
				for (int j = 0; j < Map.Length; j++)
				{
					if (Map[j][i] != null && Edge.Equals(Map[j][i]))
						if (j == Map.Length - 1)
							Console.WriteLine("e");
						else
							Console.Write("e");
					else if (!Map[j][i].Equals(new Piece()))
					{
						Piece p = Map[j][i];
						if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("┼");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("┴");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("├");
						else if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("┤");
						else if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("┬");
						else if (p.ConnectBottom == false && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == true)
							Console.Write("└");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("┘");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("─");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("┌");
						else if (p.ConnectBottom == true && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == false)
							Console.Write("┐");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("│");
						else if (p.ConnectBottom == true && p.ConnectLeft == false && p.ConnectRight == false && p.ConnectTop == false)
							Console.Write("^");
						else if (p.ConnectBottom == false && p.ConnectLeft == false && p.ConnectRight == false && p.ConnectTop == true)
							Console.Write("v");
						else if (p.ConnectBottom == false && p.ConnectLeft == true && p.ConnectRight == false && p.ConnectTop == false)
							Console.Write("]");
						else if (p.ConnectBottom == false && p.ConnectLeft == false && p.ConnectRight == true && p.ConnectTop == false)
							Console.Write("[");
					}
					else
						Console.Write(".");
				}
			}
		}
	}
}