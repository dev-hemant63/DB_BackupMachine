namespace FSM.Models
{
    public class ConnectionStrings
    {
        public string Local_DB { get; set; }
        public string Live_DB { get; set; }
    }

    public class AppConfig
    {
        public string Mode { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
    public class ConnectionConfig
    {
        public string SqlConnection { get; set; }
    }
}
