using System;

namespace WS.ExtraFunctions
{
    public static class Utilities
    {
        public static string PasswordGenerator()
        {
            Random rand = new Random();
            char[] pass = new char[12];
            pass[0] = (char)rand.Next('a', 'z' + 1);
            pass[1] = (char)rand.Next('A', 'Z' + 1);
            pass[2] = (char)rand.Next('0', '9' + 1);
            pass[3] = (char)rand.Next(33, 48);
            for(int i = 4; i < pass.Length; i++)
            {
                pass[i] = (char)rand.Next(33, 127);
            }

            for(int i = 0; i<pass.Length; i++)
            {
                int a = rand.Next(0, i + 1);
                char temp = pass[i];
                pass[i] = pass[a];
                pass[a] = temp;
            }

            string result = new string(pass);
            return result;
        }
    }
}
