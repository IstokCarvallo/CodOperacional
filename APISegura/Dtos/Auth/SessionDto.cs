namespace APISegura.Dtos.Auth
{
    public class SessionDto
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
        public bool IsCurrent { get; set; }
    }
}
