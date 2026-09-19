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

#region Question 4 - Answer Class
public class Answer : ICloneable, IComparable<Answer>
{
    public int AnswerId { get; set; }
    public string AnswerText { get; set; }

    public Answer() : this(0, string.Empty) { }

    public Answer(int answerId, string answerText)
    {
        AnswerId = answerId;
        AnswerText = answerText;
    }

    public object Clone()
    {
        return new Answer(AnswerId, AnswerText);
    }

    public int CompareTo(Answer other)
    {
        if (other == null)
        {
            return 1;
        }

        return AnswerId.CompareTo(other.AnswerId);
    }

    public override string ToString()
    {
        return $"{AnswerId}- {AnswerText}";
    }
}
#endregion

#region Question 3 - Question Types
public class TrueFalseQuestion : Question
{
    public TrueFalseQuestion() : this(string.Empty, 1) { }

    public TrueFalseQuestion(string body, int mark)
        : base("True or False Question", body, mark) { }
}

public class MCQQuestion : Question
{
    public MCQQuestion() : this(string.Empty, 1) { }

    public MCQQuestion(string body, int mark)
        : base("MCQ Question", body, mark) { }
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
