namespace Server.DTO.UserProject
{
    public class CreateUserProjectDto
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public Data.Enums.TypeCooperation TypeCooperation { get; set; }
        public decimal? FixedPrice { get; set; }
        public byte? PercentPrice { get; set; }
    }
}
