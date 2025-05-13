namespace Server.DTO.UserProject
{
    public class UpdateUserProjectDto
    {
        public Data.Enums.TypeCooperation TypeCooperation { get; set; }
        public decimal? FixedPrice { get; set; }
        public byte? PercentPrice { get; set; }
    }
}
