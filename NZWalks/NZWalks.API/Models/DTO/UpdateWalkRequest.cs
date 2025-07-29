namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequest
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid Regionid { get; set; }
        public Guid WalkDifficultyId { get; set; }
    }
}
