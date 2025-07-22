namespace NZWalks.API.Models
{
    public class Walk
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
