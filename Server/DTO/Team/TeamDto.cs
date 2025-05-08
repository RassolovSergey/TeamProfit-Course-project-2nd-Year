namespace Server.DTO.Team
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int AdminId{ get; set; }
        public int MembersCount { get; set; }
    }
}
