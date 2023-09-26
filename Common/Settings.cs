namespace Common
{
    public class Settings
    {
        // pentru a gasi portul liber
        // netstat -an |find /i "listening"
        public static int BROKER_PORT = 9000;
        public static string BROKER_IP = "127.0.0.1";

    }
}