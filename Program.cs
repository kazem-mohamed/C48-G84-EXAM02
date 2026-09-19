#region Question 1 - Question Class
public abstract partial class Question : IComparable<Question>
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
        return $"{Header}:   Mark {Mark}";
    }
}
#endregion

#region Question 2 - Exam Types
public enum ExamType
{
    Practical = 1,
    Final = 2,
}

public class FinalExam
{
}

public class PracticalExam
{
}
#endregion

public class Program
{
    public static void Main(string[] args)
    {
    }
}
