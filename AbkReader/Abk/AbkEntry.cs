
namespace AbkReaderToPgn
{
    public struct AbkEntry
    {
        public byte From { get; set; }
        public byte To { get; set; }
        public byte Promotion { get; set; }
        public byte Priority { get; set; }
        public int Games { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int PlyCount { get; set; }
        public int NextMove { get; set; }
        public int NextSibling { get; set; } // Knoten für Brothers bzw Sisters in einer Struktur

        
    }
}
