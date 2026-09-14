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

#region Question 7 - Subject Class
public class Subject : ICloneable, IComparable<Subject>
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public Exam SubjectExam { get; set; }

    public Subject() : this(0, string.Empty) { }

    public Subject(int subjectId, string subjectName)
    {
        SubjectId = subjectId;
        SubjectName = subjectName;
    }

    // Creates the exam of this subject and keeps it associated with the subject.
    public Exam CreateExam(string examType, TimeSpan timeOfExam, Question[] questions)
    {
        if (string.Equals(examType, "Practical", StringComparison.OrdinalIgnoreCase))
        {
            SubjectExam = new PracticalExam(timeOfExam, questions);
        }
        else
        {
            SubjectExam = new FinalExam(timeOfExam, questions);
        }

        return SubjectExam;
    }

    public object Clone()
    {
        return new Subject(SubjectId, SubjectName) { SubjectExam = SubjectExam };
    }

    public int CompareTo(Subject other)
    {
        if (other == null)
        {
            return 1;
        }

        return SubjectId.CompareTo(other.SubjectId);
    }

    public override string ToString()
    {
        return $"Subject {SubjectId}: {SubjectName}";
    }
}
#endregion

#region Question 6 - Exam Base Class
public abstract class Exam : ICloneable, IComparable<Exam>
{
    public TimeSpan TimeOfExam { get; set; }
    public int NumberOfQuestions { get; set; }
    public Question[] Questions { get; set; } = Array.Empty<Question>();

    protected Exam() : this(TimeSpan.FromHours(1), Array.Empty<Question>()) { }

    protected Exam(TimeSpan timeOfExam, Question[] questions)
    {
        TimeOfExam = timeOfExam;
        Questions = questions ?? Array.Empty<Question>();
        NumberOfQuestions = Questions.Length;
    }

    // Implemented differently by each exam type (requirements 8 and 9).
    public abstract void ShowExam();

    // Shared helper: asks every question and returns the answer id chosen for each one.
    protected int[] AskQuestions()
    {
        int[] chosenAnswers = new int[Questions.Length];

        for (int i = 0; i < Questions.Length; i++)
        {
            Question question = Questions[i];
            Console.WriteLine($"Q{i + 1}. {question}");

            foreach (Answer answer in question.AnswerList)
            {
                Console.WriteLine($"   {answer}");
            }

            Console.Write("Your answer: ");
            int.TryParse(Console.ReadLine(), out chosenAnswers[i]);
            Console.WriteLine();
        }

        return chosenAnswers;
    }

    public virtual object Clone()
    {
        return MemberwiseClone();
    }

    public int CompareTo(Exam other)
    {
        if (other == null)
        {
            return 1;
        }

        return NumberOfQuestions.CompareTo(other.NumberOfQuestions);
    }

    public override string ToString()
    {
        return $"{GetType().Name} - Time: {TimeOfExam.TotalMinutes} minutes - Questions: {NumberOfQuestions}";
    }
}
#endregion

#region Question 5 - Question Answers
// Second part of the partial Question class: every question carries its list of
// answers and the one that is correct.
public abstract partial class Question
{
    public Answer[] AnswerList { get; set; } = Array.Empty<Answer>();

    public Answer RightAnswer { get; set; }
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
public class FinalExam : Exam
{
    public FinalExam() { }

    public FinalExam(TimeSpan timeOfExam, Question[] questions)
        : base(timeOfExam, questions) { }

    public override void ShowExam()
    {
        Console.WriteLine(this);
    }
}

public class PracticalExam : Exam
{
    public PracticalExam() { }

    public PracticalExam(TimeSpan timeOfExam, Question[] questions)
        : base(timeOfExam, questions) { }

    #region Question 8 - Practical Exam Shows The Right Answer
    public override void ShowExam()
    {
        Console.WriteLine($"===== {this} =====");
        Console.WriteLine();

        AskQuestions();

        Console.WriteLine("----- Right Answers -----");
        for (int i = 0; i < Questions.Length; i++)
        {
            Console.WriteLine($"Q{i + 1}. {Questions[i].Header} -> {Questions[i].RightAnswer}");
        }
    }
    #endregion
}
#endregion

public class Program
{
    public static void Main(string[] args)
    {
    }
}
