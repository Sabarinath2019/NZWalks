namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequest
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid Regionid { get; set; }
        public Guid WalkDifficultyId { get; set; }
    }
}
