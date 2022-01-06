using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AbkReaderToPgn
{
    class AbkReader
    {
        #region private member
        private int linelength = 0;
        private int[] node = new int[1000];
        #endregion

        public void ReadAbkFile(string fileName)
        {
            var result = new List<AbkEntry>();
            var fInfo = new FileInfo(fileName);
            var filesize = fInfo.Length;
            byte[] test = new byte[filesize];
            var currentIndex = 900;
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                var abkSize = Marshal.SizeOf(new AbkEntry());
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    binaryReader.BaseStream.Seek(currentIndex * 28, SeekOrigin.Begin);

                    while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                    {
                        var entry = new AbkEntry();
                        entry.From = binaryReader.ReadByte();
                        entry.To = binaryReader.ReadByte();
                        entry.Promotion = binaryReader.ReadByte();
                        entry.Priority = binaryReader.ReadByte();
                        entry.Games = binaryReader.ReadInt32();
                        entry.Won = binaryReader.ReadInt32();
                        entry.Lost = binaryReader.ReadInt32();
                        entry.PlyCount = binaryReader.ReadInt32();
                        entry.NextMove = binaryReader.ReadInt32() - 900;
                        entry.NextSibling = binaryReader.ReadInt32() - 900;

                        result.Add(entry);
                    }
                    binaryReader.Close();
                    binaryReader.Dispose();
                }
                fileStream.Close();
                fileStream.Dispose();
            }

            var ply = 0;
           
            AbkMove[] moves = new AbkMove[1000];
            var gameNum = 1;
            print_head(gameNum);

            for (; ; )
            {
                if (ply < 0) break;
                var data = result[node[ply]];
                print_move(data.From, data.To, data.Promotion, ply);
                if (data.NextMove > 0)
                {
                    moves[ply].move_from = data.From;
                    moves[ply].move_to = data.To;
                    moves[ply].move_promo = data.Promotion;
                    node[ply + 1] = data.NextMove;
                    ply++;
                }
                else
                {

                    Console.Write("\n");
                    node[ply] = result[node[ply]].NextSibling;
                    while (node[ply] < 0)
                    {
                        ply--;
                        if (ply < 0)
                        {
                            break;
                        }
                        node[ply] = result[node[ply]].NextSibling; //book[node[ply]].next_sibling;
                    }
                    if (ply < 0) break;
                    gameNum++;
                    print_head(gameNum);
                    linelength = 0;
                    for (int i = 0; i < ply; i++)
                    {
                        print_move(moves[i].move_from, moves[i].move_to, moves[i].move_promo, i);
                    }
                }
            }
        }
        void print_head(int gameNum)
        {
            Console.Write("*\n\n");
            Console.Write("[Event \"?\"]\n");
            Console.Write("[Site \"?\"]\n");
            Console.Write("[Date \"????.??.??\"]\n");
            Console.Write("[Round \"?\"]\n");
            Console.Write("[Result \"*\"]\n");
            Console.Write("[White \"?\"]\n");
            Console.Write("[Black \"?\"]\n");
            Console.Write("[GameNum "+ gameNum + "]\n\n");
        }
        void print_move(byte move_from, byte move_to, byte move_promo, int ply)
        {
            if ((move_from >> 3) + 1 == 32 && (move_to >> 3) + 1 == 32) return;
            if (linelength > 80)
            {
                Console.Write("\n");
                linelength = 0;
            }
            if ((ply & 1) == 0)
            {
                Console.Write(string.Format("{0}.", ply / 2 + 1));
                linelength += 3;
            }

            Console.Write(string.Format("{0}{1}{2}{3}",
                    GetCharacter(Convert.ToSByte((move_from & 7) + 'a')), GetCharacter(Convert.ToSByte((move_from >> 3) + 1)),
                    GetCharacter(Convert.ToSByte((move_to & 7) + 'a')), GetCharacter(Convert.ToSByte((move_to >> 3) + 1))));

            if (move_promo == '1')
            {
                char[] prm = new char[] { 'R', 'N', 'B', 'Q' };
                Console.Write(string.Format("{0}", prm[Math.Abs(Convert.ToInt16(move_promo)) - 1]));
            }
            Console.Write(" ");
            linelength += 5;
        }
        private string GetCharacter(sbyte val)
        {
            var result = Convert.ToChar(val);
            if (char.IsControl(result))
            {
                return Regex.Replace(Convert.ToString(val), @"[^\u0000-\u007F]", String.Empty);
            }
            else return result.ToString();
        }
    }
}
