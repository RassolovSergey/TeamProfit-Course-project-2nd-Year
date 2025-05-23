namespace Server.Options
{
    public class FixerApiOptions
    {
        public string BaseUrl { get; set; } = null!;
        public string BaseCurrency { get; set; } = "EUR";
        public int UpdateIntervalMinutes { get; set; } = 60;
        public string AccessKey { get; set; } = null!;
    }
}
