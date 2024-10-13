using System.Text.RegularExpressions;

namespace Thread_Synchronization
{
    internal class Program
    {
        static int numOfFile = 1;
        public static int amountOfWords = 0;
        public static int amountOfStrings = 0;
        public static int amountOfPunktuationMarks = 0;
        public class Statistic
        {
            public static string fileName = @$"C:\Users\User\Desktop\texts\text{numOfFile}.txt";
            static StreamReader myFile = new StreamReader(fileName);
            string text = myFile.ReadToEnd();
            public Statistic() { }
            public void WordsCount()
            {
                lock (this)
                {
                    string pattern = @"\w+";
                    var matches = Regex.Matches(text, pattern);
                    amountOfWords = +matches.Count();
                }
            }
            public void LineCount()
            {
                lock (this)
                {
                    string pattern = "\n";
                    var matches = Regex.Matches(text, pattern);
                    amountOfStrings += matches.Count() + 1;
                }
            }
            public void PunktuationMarksCount()
            {
                lock (this)
                {
                    string pattern = @"[^\w\s]";
                    var matches = Regex.Matches(text, pattern);
                    amountOfPunktuationMarks += matches.Count() + 1;
                }
            }
        }
        static void Main(string[] args)
        {
            Statistic statistic = new Statistic();

            Thread[] threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    statistic.WordsCount();
                    statistic.LineCount();
                    statistic.PunktuationMarksCount();
                    numOfFile++;

                });
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
            Console.WriteLine($"Amount of words: {amountOfWords}.\n" +
            $"Amount of strings: {amountOfStrings}.\n" +
            $"Amount of punctuation marks: {amountOfPunktuationMarks}.");
        }
    }
}
