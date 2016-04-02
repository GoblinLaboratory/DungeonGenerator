using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DungeonGenerator
{
	public class CreateDungeon
	{
		enum Moves { Up, Down, Left, Right, None };
		static Piece[] Pieces = new Piece[16]
			{
			new Piece(false, false, false, false),  //none
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
			Random random = new Random();
			int w = random.Next(1, m.Length - 1);
			int h = random.Next(1, m[0].Length - 1);
			bool placing = true;
			bool NoMoves = false;
			

			while(placing)
			{
				var LastMove = Moves.None;

				Piece Above = m[w][h - 1];
				Piece Below = m[w][h + 1];
				Piece Left = m[w - 1][h];
				Piece Right = m[w + 1][h];
				Piece cPiece = m[w][h];

				//placing a piece
				if (cPiece.Equals(new Piece()))
				{
					List<Piece> select = Pieces.ToList();
					#region LINQ
					if (Below.ConnectTop == true)
					{
						select = select
						.Where(p => p.ConnectBottom == true)
						.Select(p => p).ToList();
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

					if (Left.ConnectRight == true)
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
						cPiece = tPiece;
					}
						
					else
						NoMoves = true;
				}
				//moving to a new block
				else if(cPiece.GetType() == new Piece().GetType())
				{
					if (Left.Equals(new Piece()) && cPiece.ConnectLeft == true)
						w -= 1;
					else if (Right.Equals(new Piece()) && cPiece.ConnectRight == true)
						w += 1;
					else if (Above.Equals(new Piece()) && cPiece.ConnectTop == true)
						h -= 1;
					else if (Below.Equals(new Piece()) && cPiece.ConnectBottom == true)
						h += 1;
					else
						NoMoves = true;
				}
				else if(NoMoves)
				{
					while(NoMoves)
					{
						if (cPiece.Equals(new Piece()) && Above.ConnectBottom == false && Left.ConnectRight == false && Right.ConnectLeft == false && Below.ConnectTop == false)
						{
							placing = false;
							NoMoves = false;
						}
						else if((cPiece.ConnectTop == true && Above.ConnectBottom == true)||(cPiece.ConnectLeft == true && Left.ConnectRight == true) ||(cPiece.ConnectRight == true && Right.ConnectLeft == true) ||(cPiece.ConnectBottom == true && Below.ConnectTop == true))
						{
							NoMoves = false;
						}
						else
						{
							int PossibleMoves = 0;

							if (cPiece.ConnectBottom == true)
								PossibleMoves += 1;
							if (cPiece.ConnectLeft == true)
								PossibleMoves += 1;
							if (cPiece.ConnectRight == true)
								PossibleMoves += 1;
							if (cPiece.ConnectTop == true)
								PossibleMoves += 1;

							if (LastMove == Moves.None)
							{
								if (cPiece.ConnectBottom == true)
								{
									h += 1;
									LastMove = Moves.Down;
								}
								else if (cPiece.ConnectLeft == true)
								{
									w -= 1;
									LastMove = Moves.Left;
								}
								else if (cPiece.ConnectRight == true)
								{
									w += 1;
									LastMove = Moves.Right;
								}
								else if (cPiece.ConnectTop == true)
								{
									h -= 1;
									LastMove = Moves.Up;
								}
							}
							else if (PossibleMoves == 4)
							{
								int r = random.Next(0, 2);
								if (LastMove == Moves.Down)
								{
									switch (r)
									{
										case 0:
											h += 1;
											LastMove = Moves.Down;
											break;
										case 1:
											w -= 1;
											LastMove = Moves.Left;
											break;
										case 2:
											w += 1;
											LastMove = Moves.Right;
											break;
										default:
											break;
									}
								}

								else if (LastMove == Moves.Up)
								{
									switch (r)
									{
										case 0:
											w -= 1;
											LastMove = Moves.Left;
											break;
										case 1:
											w += 1;
											LastMove = Moves.Right;
											break;
										case 2:
											h -= 1;
											LastMove = Moves.Up;
											break;
										default:
											break;
									}
								}

								else if (LastMove == Moves.Left)
								{
									switch (r)
									{
										case 0:
											h += 1;
											LastMove = Moves.Down;
											break;
										case 1:
											w -= 1;
											LastMove = Moves.Left;
											break;
										case 2:
											h -= 1;
											LastMove = Moves.Up;
											break;
										default:
											break;
									}
								}

								else if (LastMove == Moves.Right)
								{
									switch (r)
									{
										case 0:
											h += 1;
											LastMove = Moves.Down;
											break;
										case 1:
											w += 1;
											LastMove = Moves.Right;
											break;
										case 2:
											h -= 1;
											LastMove = Moves.Up;
											break;
										default:
											break;
									}
								}
							}

							else if(PossibleMoves == 3)
							{
								int r = random.Next(0, 2);
								if (LastMove == Moves.Down)
								{
									switch (r)
									{
										case 0:
											if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											else if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											break;

										case 1:
											if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											else if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											break;

										case 2:
											if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											else if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											else if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											break;

										default:
											break;
									}
								}

								else if (LastMove == Moves.Up)
								{
									switch (r)
									{
										case 0:
											if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											else if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}

											break;
										case 1:
											if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											else if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											
											break;
										case 2:
											if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											else if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											break;
										default:
											break;
									}
								}

								else if (LastMove == Moves.Left)
								{
									switch (r)
									{
										case 0:
											if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											else if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											else if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
												break;
										case 1:
											if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											else if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											break;
										case 2:
											if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											else if (cPiece.ConnectLeft == true)
											{
												w -= 1;
												LastMove = Moves.Left;
											}
											else if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											break;
										default:
											break;
									}
								}

								else if (LastMove == Moves.Right)
								{
									switch (r)
									{
										case 0:
											if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											else if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											break;
										case 1:
											if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											else if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											else if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											break;
										case 2:
											if (cPiece.ConnectTop == true)
											{
												h -= 1;
												LastMove = Moves.Up;
											}
											else if (cPiece.ConnectBottom == true)
											{
												h += 1;
												LastMove = Moves.Down;
											}
											else if (cPiece.ConnectRight == true)
											{
												w += 1;
												LastMove = Moves.Right;
											}
											break;
										default:
											break;
									}
								}
							}

							else if (PossibleMoves == 2)
							{
								
								if (LastMove == Moves.Down)
								{
									if (cPiece.ConnectBottom == true)
									{
										h += 1;
										LastMove = Moves.Down;
									}
									else if (cPiece.ConnectLeft == true)
									{
										w -= 1;
										LastMove = Moves.Left;
									}
									else if (cPiece.ConnectRight == true)
									{
										w += 1;
										LastMove = Moves.Right;
									}
									
								}

								else if (LastMove == Moves.Up)
								{
									if (cPiece.ConnectLeft == true)
									{
										w -= 1;
										LastMove = Moves.Left;
									}
									else if (cPiece.ConnectRight == true)
									{
										w += 1;
										LastMove = Moves.Right;
									}
									else if (cPiece.ConnectTop == true)
									{
										h -= 1;
										LastMove = Moves.Up;
									}
								}

								else if (LastMove == Moves.Left)
								{
									if (cPiece.ConnectBottom == true)
									{
										h += 1;
										LastMove = Moves.Down;
									}
									else if (cPiece.ConnectLeft == true)
									{
										w -= 1;
										LastMove = Moves.Left;
									}
									else if (cPiece.ConnectTop == true)
									{
										h -= 1;
										LastMove = Moves.Up;
									}
								}

								else if (LastMove == Moves.Right)
								{
									if (cPiece.ConnectBottom == true)
									{
										h += 1;
										LastMove = Moves.Down;
									}
									else if (cPiece.ConnectRight == true)
									{
										w += 1;
										LastMove = Moves.Right;
									}
									else if (cPiece.ConnectTop == true)
									{
										h -= 1;
										LastMove = Moves.Up;
									}
								}
							}
							
							else if (PossibleMoves == 1)
							{
								if (cPiece.ConnectBottom == true)
								{
									h += 1;
									LastMove = Moves.Down;
								}
								else if (cPiece.ConnectLeft == true)
								{
									w -= 1;
									LastMove = Moves.Left;
								}
								else if (cPiece.ConnectRight == true)
								{
									w += 1;
									LastMove = Moves.Right;
								}
								else if (cPiece.ConnectTop == true)
								{
									h -= 1;
									LastMove = Moves.Up;
								}	
							}
							else
							{
								placing = false;
							}
						}
						
					}
				}
				else
				{

				}
			}
			return m;
		}
	}
}
