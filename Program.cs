#region Console Input Helpers
public static class ConsoleInput
{
    public static string ReadText(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();

            if (input == null)
            {
                return string.Empty;
            }

            if (input.Trim().Length > 0)
            {
                return input.Trim();
            }

            Console.WriteLine("Value cannot be empty.");
        }
    }

    public static int ReadNumber(string prompt, int min, int max)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();

            if (input == null)
            {
                return min;
            }

            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Please enter a number between {min} and {max}.");
        }
    }
}
#endregion

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

    // Set by Subject.CreateExam.
    public Subject Subject { get; set; }

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

    #region Question 8 - Practical Exam Shows The Right Answer
    public override void ShowExam()
    {
        Console.WriteLine("Practical Exam");

        int[] chosenAnswers = AskQuestions();

        Console.WriteLine("Practical Exam Results:");

        for (int i = 0; i < Questions.Length; i++)
        {
            Question question = Questions[i];

            Console.WriteLine($"Question {i + 1}: {question.Body}");
            Console.WriteLine($"Your Answer => {question.TextOfAnswer(chosenAnswers[i])}");
            Console.WriteLine($"Correct Answer => {question.RightAnswer.AnswerText}");
            Console.WriteLine();
        }

        ShowGrade(chosenAnswers);
    }
    #endregion
}
#endregion

#region Question 7 - Subject Class
public class Subject : ICloneable, IComparable<Subject>
{
    private const int ChoicesPerMCQ = 4;

    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public Exam SubjectExam { get; set; }

    public Subject() : this(0, string.Empty) { }

    public Subject(int subjectId, string subjectName)
    {
        SubjectId = subjectId;
        SubjectName = subjectName;
    }

    // Asks for the exam data, then builds and stores it.
    public Exam CreateExam()
    {
        ExamType examType = (ExamType)ConsoleInput.ReadNumber(
            "Enter the type of exam (1 for Practical, 2 for Final):", 1, 2);

        int minutes = ConsoleInput.ReadNumber(
            "Please enter the time for the exam (30 to 180 minutes):", 30, 180);

        int numberOfQuestions = ConsoleInput.ReadNumber(
            "Please enter the number of questions:", 1, 50);

        Question[] questions = new Question[numberOfQuestions];

        for (int i = 0; i < numberOfQuestions; i++)
        {
            questions[i] = ReadQuestion(examType);
        }

        SubjectExam = examType == ExamType.Practical
            ? new PracticalExam(TimeSpan.FromMinutes(minutes), questions)
            : new FinalExam(TimeSpan.FromMinutes(minutes), questions);

        SubjectExam.Subject = this;
        return SubjectExam;
    }

    // Practical exams accept MCQ only, final exams accept both question types.
    private static Question ReadQuestion(ExamType examType)
    {
        bool isTrueFalse = examType == ExamType.Final &&
            ConsoleInput.ReadNumber(
                "Enter the type of question (1 for True or False, 2 for MCQ):", 1, 2) == 1;

        string body = ConsoleInput.ReadText("Please enter the question body:");
        int mark = ConsoleInput.ReadNumber("Please enter the question mark:", 1, 100);

        if (isTrueFalse)
        {
            int trueFalseAnswer = ConsoleInput.ReadNumber(
                "Please enter the ID of the correct answer (1 for True, 2 for False):", 1, 2);

            return new TrueFalseQuestion(body, mark, trueFalseAnswer == 1);
        }

        Console.WriteLine("Choices of Question:");
        string[] choices = new string[ChoicesPerMCQ];

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i] = ConsoleInput.ReadText($"Please enter choice number {i + 1}:");
        }

        int correctAnswerId = ConsoleInput.ReadNumber(
            $"Please enter the ID of the correct answer (1 to {ChoicesPerMCQ}):", 1, ChoicesPerMCQ);

        MCQQuestion question = new MCQQuestion(body, mark);
        question.SetAnswers(correctAnswerId, choices);

        return question;
    }

    public object Clone()
    {
        Subject copy = new Subject(SubjectId, SubjectName);

        if (SubjectExam != null)
        {
            // The cloned exam points back at the cloned subject, not the original one.
            copy.SubjectExam = (Exam)SubjectExam.Clone();
            copy.SubjectExam.Subject = copy;
        }

        return copy;
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

public class Program
{
    public static void Main(string[] args)
    {
    }
}
