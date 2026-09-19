#region Question 1 - Question Class
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

    // Deep copy: the copy gets its own answers, and its RightAnswer points inside them.
    public virtual object Clone()
    {
        Question copy = (Question)MemberwiseClone();
        copy.AnswerList = new Answer[AnswerList.Length];

        for (int i = 0; i < AnswerList.Length; i++)
        {
            copy.AnswerList[i] = (Answer)AnswerList[i].Clone();

            if (RightAnswer != null && AnswerList[i].AnswerId == RightAnswer.AnswerId)
            {
                copy.RightAnswer = copy.AnswerList[i];
            }
        }

        return copy;
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

#region Question 5 - Question Answers
public abstract partial class Question
{
    public Answer[] AnswerList { get; set; } = Array.Empty<Answer>();

    public Answer RightAnswer { get; set; }

    // Answers are numbered from 1, so rightAnswerId is the number the student types.
    public void SetAnswers(int rightAnswerId, params string[] answerTexts)
    {
        AnswerList = new Answer[answerTexts.Length];

        for (int i = 0; i < answerTexts.Length; i++)
        {
            AnswerList[i] = new Answer(i + 1, answerTexts[i]);
        }

        RightAnswer = Array.Find(AnswerList, answer => answer.AnswerId == rightAnswerId);
    }

    public bool IsAnsweredCorrectly(int answerId)
    {
        return RightAnswer != null && RightAnswer.AnswerId == answerId;
    }

    public string TextOfAnswer(int answerId)
    {
        Answer answer = Array.Find(AnswerList, item => item.AnswerId == answerId);
        return answer == null ? "No answer" : answer.AnswerText;
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
    public TrueFalseQuestion() : this(string.Empty, 1, true) { }

    public TrueFalseQuestion(string body, int mark, bool rightAnswer)
        : base("True or False Question", body, mark)
    {
        SetAnswers(rightAnswer ? 1 : 2, "True", "False");
    }
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
