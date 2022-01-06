using System;
using System.Collections.Generic;
using System.Text;


namespace AbkReaderToPgn
{
  
    public class IntermediateEntry
    {        
        public int Ply { get; set; }
        public List<IntermediateEntry> Children { get; set; }
        public int WhiteWins { get; set; }
        public int BlackWins { get; set; }
        public int Draws { get; set; }
        public int TotalGames => WhiteWins + BlackWins + Draws;
    }
}
