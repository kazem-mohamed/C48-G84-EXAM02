#region Question 1 - Question Class
// Base class for every question type. It is declared partial so the answer members
// required by requirement 5 can live in their own region.
public abstract partial class Question : ICloneable, IComparable<Question>
{
    public string Header { get; set; }
    public string Body { get; set; }
    public int Mark { get; set; }

    protected Question() : this("Question", string.Empty, 1) { }

    protected Question(string header, string body) : this(header, body, 1) { }

    protected Question(string header, string body, int mark)
    {
        Header = header;
        Body = body;
        Mark = mark;
    }

    public virtual object Clone()
    {
        return MemberwiseClone();
    }

    public int CompareTo(Question other)
    {
        if (other == null)
        {
            return 1;
        }

        return Mark.CompareTo(other.Mark);
    }

    public override string ToString()
    {
        return $"{Header} ({Mark} Marks){Environment.NewLine}{Body}";
    }
}
#endregion

public class Program
{
    public static void Main(string[] args)
    {
    }
}
