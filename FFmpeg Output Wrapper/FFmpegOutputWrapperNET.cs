using System.Text;
using System;

public class FFmpegOutputWrapperNET
{
    public String Frames = String.Empty;
    public String Q = String.Empty;
    public String Fps = String.Empty;
    public size Size;

    public struct size
    {
        public Double Bytes { get; set; }
        public Double Kilobytes { get; set; }
        public Double Megabytes { get; set; }
        public Double Gigabytes { get; set; }
    }

    public TimeSpan Time;
    public String Bitrate = String.Empty;
    public String Speed = String.Empty;
    public String EXCEPTIONS = String.Empty;
    public String Output = String.Empty;

    public FFmpegOutputWrapperNET(String OutputFFmpeg)
    {
        FrameLineWrapper(OutputFFmpeg);
    }

    void GetSize(String size)
    {
        if (String.IsNullOrWhiteSpace(size) == false)
        {
            var temp = size.ToLower().Trim();
            if (temp.Contains("kb"))
            {
                temp = temp.Replace("kb", String.Empty);
                Size = new size()
                {
                    Bytes = Convert.ToDouble(temp) * 1024.0,
                    Kilobytes = Convert.ToDouble(temp),
                    Megabytes = Math.Round(Convert.ToDouble(temp) / 1024.0, 2),
                    Gigabytes = Math.Round(Convert.ToDouble(temp) / 1024.0 / 1024.0, 2)
                };
            }
        }
    }

    public void FrameLineWrapper(String OutputFFmpeg)
    {
        if (String.IsNullOrWhiteSpace(OutputFFmpeg) == false)
        {
            if (OutputFFmpeg.Trim().ToLower().StartsWith("frame") || OutputFFmpeg.Trim().ToLower().StartsWith("fps") || OutputFFmpeg.Trim().ToLower().StartsWith("q") || OutputFFmpeg.Trim().ToLower().StartsWith("size") || OutputFFmpeg.Trim().ToLower().StartsWith("time") || OutputFFmpeg.Trim().ToLower().StartsWith("bitrate") || OutputFFmpeg.Trim().ToLower().StartsWith("speed"))
            {
                String[] Info = RemoveSpaces(OutputFFmpeg).Split(',');
                foreach (String s in Info)
                {
                    try
                    {
                        if (s.ToLower().Contains("frame"))
                        {
                            Frames = RemoveUntilFirstSymbol(s);
                        }
                        if (s.ToLower().Contains("fps"))
                        {
                            Fps = RemoveUntilFirstSymbol(s);
                        }
                        if (s.ToLower().Contains("q"))
                        {
                            Q = RemoveUntilFirstSymbol(s);
                        }
                        if (s.ToLower().Contains("size"))
                        {
                            GetSize(RemoveUntilFirstSymbol(s));
                        }
                        if (s.ToLower().Contains("bitrate"))
                        {
                            Bitrate = RemoveUntilFirstSymbol(s);
                        }
                        if (s.ToLower().Contains("speed"))
                        {
                            Speed = RemoveUntilFirstSymbol(s);
                        }
                        if (s.ToLower().Contains("time"))
                        {
                            TimeSpan t = TimeSpan.Parse(RemoveUntilFirstSymbol(s));
                            Time = new TimeSpan(t.Hours, t.Minutes, t.Seconds);
                        }
                    }
                    catch (Exception ex)
                    {
                        EXCEPTIONS = ex.Message;
                    }
                }
            }
            else
            {
                Output = OutputFFmpeg;
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