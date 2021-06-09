using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AVRTray
{
    class AVR
    {
        private TcpClient client;
        private int _readDelay = 500;
        private readonly string[] sources = {
            "TUNER", "DVD", "BD", "TV", "SAT", "SAT/CBL", "MPLAY", "GAME", "AUX1", "NET", "PANDORA", "SIRIUSXM",
            "LASTFM", "FLICKR", "FAVORITES", "IRADIO", "SERVER", "USB/IPOD", "USB", "IPD", "IRP", "FVP"
        };

        // User editable (in the future)
        public string host = "10.0.0.6";
        public int port = 23;
        public Dictionary<string, string> CustomSources = new()
        {
            { "PC", "AUX1" },
            { "Turntable", "MPLAY" },
            { "MiniDisc", "SAT/CBL" },
        };

        public AVR()
        {
            client = new TcpClient(host, port);
        }

        private void CheckConnection()
        {
            if (client == null)
                Connect();
            else if (!client.Connected)
                Connect();
        }

        public bool IsConnected()
        {
            if (client == null)
                return false;

            return client.Connected;
        }
        public void Disconnect()
        {
            client.Close();
        }

        public void Connect()
        {
            try
            {
                client = new TcpClient(host, port);
            }
            catch (Exception)
            {
            }
        }

        public void Write(string cmd)
        {
            cmd += Environment.NewLine;

            if (!IsConnected()) Connect();
            var buf = Encoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
            Debug.WriteLine($"Sent: [{ cmd.Replace(Environment.NewLine, ",") }]");
            client.GetStream().Write(buf, 0, buf.Length);
        }

        public string Read()
        {
            if (!IsConnected()) Connect();
            StringBuilder sb = new ();
            do {
                ParseCommmand(sb);
                // Sleep required as AVR tends to have variable delay
                Thread.Sleep(_readDelay);
            } while (client.Available > 0);
            Debug.WriteLine($"Received: [{ sb.ToString().Replace("\r", ",") }]");

            return sb.ToString();
        }

        internal enum Verbs
        {
            WILL = 251,
            WONT = 252,
            DO = 253,
            DONT = 254,
            IAC = 255
        }
        internal enum Options
        {
            SGA = 3
        }

        // Not gonna lie, not idea what any of this does or how it works
        private void ParseCommmand(StringBuilder sb)
        {
            while (client.Available > 0)
            {
                var input = client.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command
                        var inputverb = client.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.IAC:
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO:
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                var inputoption = client.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                client.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA)
                                    client.GetStream().WriteByte(inputverb == (int)Verbs.DO
                                        ? (byte)Verbs.WILL
                                        : (byte)Verbs.DO);
                                else
                                    client.GetStream().WriteByte(inputverb == (int)Verbs.DO
                                        ? (byte)Verbs.WONT
                                        : (byte)Verbs.DONT);
                                client.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
            }
        }

        public void TogglePower()
        {
            Write("PW?");
            Write(Read().Trim() == "PWON" ? "PWSTANDBY" : "PWON");
        }

        public void ToggleMute()
        {
            Write("MU?");
            Write(Read().Trim() == "MUON" ? "MUOFF" : "MUON");
        }

        public int GetVolume()
        {
            int retries = 5;
            while (retries > 0)
            {
                Write("MV?");
                string result = Read().Split("\r")[0];
                if (result.StartsWith("MV"))
                {
                    string x = result[2..4].Trim();
                    Debug.WriteLine("Trimmed to: [" + x + "]");
                    return int.Parse(x);
                }
                retries--;
            }
            throw new Exception("F");
        }

        public string GetSource()
        {
            int retries = 5;
            while (retries > 0)
            {
                Write("SI?");
                string result = Read().Split("\r")[0];
                if (result.StartsWith("SI"))
                {
                    // Trim SI from start
                    result = result[2..];
                    // Find pretty name if it exists
                    if (CustomSources.ContainsValue(result))
                    {
                        result = CustomSources.FirstOrDefault(x => x.Value == result).Key;
                    }
                    Debug.WriteLine("Got source: " + result);
                    return result;
                }
                retries--;
            }
            throw new Exception("F");
        }

        public void SetSource(string sourceName)
        {
            if (CustomSources.ContainsKey(sourceName))
            {
                Debug.WriteLine($"Setting source to: {CustomSources[sourceName]} ({sourceName})");
                Write("SI" + CustomSources[sourceName]);
            } else
            {
                Debug.WriteLine("Setting source to: " + sourceName);
                Write("SI" + sourceName);
            }
        }
    }
}
