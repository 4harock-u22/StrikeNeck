using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.Camera
{
    internal class RandomStringGenerator
    {
        private readonly Random random = new Random();
        internal String GenerateRandomString(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_0123456789";
            return new string(
                Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }
    }
}
