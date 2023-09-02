using Newtonsoft.Json;

namespace NATSParser.Commands
{
    public record InfoCommand
    {
        public Settings Setting { get; set; }
        public override string ToString()
        {
            return $"INFO {JsonConvert.SerializeObject(Setting)}\r\n";
        }



        public class Settings
        {
            public string server_id { get; set; }
            public string version { get; set; }
            public int proto { get; set; }
            public string go { get; set; }
            public string host { get; set; }
            public int port { get; set; }
            public int max_payload { get; set; }
            public int client_id { get; set; }
        }

    }

}
