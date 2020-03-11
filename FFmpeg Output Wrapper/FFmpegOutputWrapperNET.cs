using System;
using System.Linq;
using System.Text;

namespace FFmpeg_Output_Wrapper
{
    public class FFmpegOutputWrapperNET
    {
        public String Frames = String.Empty;
        public String Q = String.Empty;
        public String Fps = String.Empty;
        public String Size = String.Empty;
        public TimeSpan Time;
        public String Bitrate = String.Empty;
        public String Speed = String.Empty;
        public String EXCEPTIONS = String.Empty;

        public FFmpegOutputWrapperNET(String OutputFFmpeg)
        {
            FrameLineWrapper(OutputFFmpeg);
        }

        public void FrameLineWrapper(String OutputFFmpeg)
        {
            if (String.IsNullOrWhiteSpace(OutputFFmpeg) == false)
            {
                if (OutputFFmpeg.Trim().StartsWith("frame"))
                {
                    String[] Info = RemoveSpaces(OutputFFmpeg).Split(',');
                    if (Info.Count() > 0)
                    {
                        String ecc = String.Empty;
                        try
                        {
                            Frames = RemoveUntilFirstSymbol(Info[0]);
                            Fps = RemoveUntilFirstSymbol(Info[1]);
                            Q = RemoveUntilFirstSymbol(Info[2]);
                            Size = RemoveUntilFirstSymbol(Info[3]);
                            TimeSpan t = TimeSpan.Parse(RemoveUntilFirstSymbol(Info[4]));
                            Time = new TimeSpan(t.Hours, t.Minutes, t.Seconds);
                            Bitrate = RemoveUntilFirstSymbol(Info[5]);
                            Speed = RemoveUntilFirstSymbol(Info[6]);
                        }
                        catch (Exception ex)
                        {
                            EXCEPTIONS = ex.Message;
                        }
                    }
                }
            }
        }

        protected String RemoveUntilFirstSymbol(String s)
        {
            for (Int32 i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]) == false)
                {
                    return s.Remove(0, i + 1);
                }
            }
            return s;
        }

        protected String RemoveSpaces(String OutputWithSpaces)
        {
            Int32 FirstSpace = 0;
            Char tmp = new Char();
            StringBuilder sb = new StringBuilder();
            foreach (Char c in OutputWithSpaces)
            {
                tmp = c;
                if (Char.IsWhiteSpace(c))
                {
                    FirstSpace++;
                    if (FirstSpace == 1)
                    {
                        tmp = ',';
                    }
                }
                else
                {
                    FirstSpace = 0;
                }
                sb.Append(tmp);
            }
            return sb.ToString().Replace(" ", "").Replace("=,", "=");
        }
    }
}
