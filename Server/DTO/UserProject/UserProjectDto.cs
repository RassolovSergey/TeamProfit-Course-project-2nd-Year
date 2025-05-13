namespace Server.DTO.UserProject
{
    public class UserProjectDto
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public Data.Enums.TypeCooperation TypeCooperation { get; set; }
        public decimal? FixedPrice { get; set; }
        public byte? PercentPrice { get; set; }
    }
}
