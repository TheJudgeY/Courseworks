using System.Text;

namespace Helpers
{
    public static class StringManipulator
    {
        public static string StringWrapper(string text)
        {
            int windowWidth = Console.WindowWidth;
            StringBuilder sb = new StringBuilder();
            StringBuilder result = new StringBuilder();

            foreach (string word in text.Split(' '))
            {
                if (sb.Length + word.Length + 1 > windowWidth)
                {
                    result.AppendLine(sb.ToString());
                    sb.Clear();
                }

                sb.Append(word + " ");
            }

            result.AppendLine(sb.ToString());

            return result.ToString();
        }
    }
}
