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
        return $"{AnswerId}) {AnswerText}";
    }
}
#endregion

#region Question 3 - Question Types
// Final exams use both types, practical exams use MCQ only.
public class TrueFalseQuestion : Question
{
    public TrueFalseQuestion() : this("True or False Question", string.Empty, 1) { }

    public TrueFalseQuestion(string header, string body, int mark)
        : base(header, body, mark) { }

    public override string ToString()
    {
        return $"[True or False] {base.ToString()}";
    }
}

public class MCQQuestion : Question
{
    public MCQQuestion() : this("MCQ Question", string.Empty, 1) { }

    public MCQQuestion(string header, string body, int mark)
        : base(header, body, mark) { }

    public override string ToString()
    {
        return $"[MCQ] {base.ToString()}";
    }
}
#endregion

#region Question 2 - Exam Types
// The system supports two exam types. They share the Exam base class added in
// requirement 6, and each one implements ShowExam in requirements 8 and 9.
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
