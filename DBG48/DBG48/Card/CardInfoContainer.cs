namespace DBG48
{
    /// <summary>
    /// CardInfoContainer
    /// </summary>
    public class CardInfoContainer
    {
        public string Filepath { get; set; }
        public string Name { get; set; }

        public uint Cost { get; private set; }

        public uint ResourcePoint { get; private set; }
        public uint AttackPoint { get; private set; }
        public uint ActionPoint { get; private set; }

        public CardInfoContainer(
            string filepath,
            string Name, 
            uint Cost = 0,
            uint ActionPoint = 0,
            uint ResourcePoint = 0,
            uint AttackPoint = 0)
        {
            this.Filepath = filepath;
            this.Name = Name;
            this.Cost = Cost;
            this.ActionPoint = ActionPoint;
            this.ResourcePoint = ResourcePoint;
            this.AttackPoint = AttackPoint;
        }
    }
}
