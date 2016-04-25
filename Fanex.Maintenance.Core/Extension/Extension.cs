// --------------------------------------------------------------------------
// <copyright file="Extension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Maintenance.Core.Extension
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Compress string to MD5 code.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>
        /// The MD5 code of input parameter.
        /// </returns>
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder builder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return builder.ToString();
        }
    }
}
