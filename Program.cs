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

#region Question 6 - Exam Base Class
public abstract class Exam : ICloneable, IComparable<Exam>
{
    public TimeSpan TimeOfExam { get; set; }
    public Question[] Questions { get; set; } = Array.Empty<Question>();

    public int NumberOfQuestions => Questions.Length;

    protected TimeSpan TakenTime { get; private set; }

    protected int TotalMarks
    {
        get
        {
            int total = 0;

            foreach (Question question in Questions)
            {
                total += question.Mark;
            }

            return total;
        }
    }

    protected Exam() : this(TimeSpan.FromHours(1), Array.Empty<Question>()) { }

    protected Exam(TimeSpan timeOfExam, Question[] questions)
    {
        TimeOfExam = timeOfExam;
        Questions = questions ?? Array.Empty<Question>();
    }

    public abstract void ShowExam();

    protected int[] AskQuestions()
    {
        int[] chosenAnswers = new int[Questions.Length];
        DateTime startedAt = DateTime.Now;

        for (int i = 0; i < Questions.Length; i++)
        {
            Question question = Questions[i];

            Console.WriteLine($"Question {i + 1}: {question.Body}");
            Console.WriteLine(question);

            foreach (Answer answer in question.AnswerList)
            {
                Console.WriteLine(answer);
            }

            chosenAnswers[i] = ReadAnswerId(question);
            Console.WriteLine();
        }

        TakenTime = DateTime.Now - startedAt;
        return chosenAnswers;
    }

    // Keeps asking until the student types one of the listed answer numbers.
    private static int ReadAnswerId(Question question)
    {
        while (true)
        {
            Console.WriteLine("Enter your answer ID:");
            string input = Console.ReadLine();

            // Nothing left to read: leave the question unanswered.
            if (input == null)
            {
                return 0;
            }

            if (int.TryParse(input, out int answerId) &&
                Array.Exists(question.AnswerList, answer => answer.AnswerId == answerId))
            {
                return answerId;
            }

            Console.WriteLine($"Please enter a number between 1 and {question.AnswerList.Length}.");
        }
    }

    protected void ShowGrade(int[] chosenAnswers)
    {
        int grade = 0;

        for (int i = 0; i < Questions.Length; i++)
        {
            if (Questions[i].IsAnsweredCorrectly(chosenAnswers[i]))
            {
                grade += Questions[i].Mark;
            }
        }

        Console.WriteLine($"Your Grade is {grade} from {TotalMarks}");
        Console.WriteLine($"Time = {TakenTime}");
        Console.WriteLine("Thank you");
    }

    public virtual object Clone()
    {
        Exam copy = (Exam)MemberwiseClone();
        copy.Questions = new Question[Questions.Length];

        for (int i = 0; i < Questions.Length; i++)
        {
            copy.Questions[i] = (Question)Questions[i].Clone();
        }

        return copy;
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

public class FinalExam : Exam
{
    public FinalExam() { }

    public FinalExam(TimeSpan timeOfExam, Question[] questions)
        : base(timeOfExam, questions) { }

    public override void ShowExam()
    {
        Console.WriteLine("Final Exam");
        AskQuestions();
    }
}

public class PracticalExam : Exam
{
    public PracticalExam() { }

    public PracticalExam(TimeSpan timeOfExam, Question[] questions)
        : base(timeOfExam, questions)
    {
        foreach (Question question in Questions)
        {
            if (question is not MCQQuestion)
            {
                throw new ArgumentException(
                    "A practical exam accepts MCQ questions only.", nameof(questions));
            }
        }
    }

    public override void ShowExam()
    {
        Console.WriteLine("Practical Exam");
        AskQuestions();
    }
}
#endregion

public class Program
{
    public static void Main(string[] args)
    {
    }
}
