using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NP.Data;
using NP.Models;

namespace NP.Search
{
    public class KMP
    {
        public static List<Book> Search(string pattern, NPDbContext db)
        {
            List<Book> books = db.Books.ToList();

            List<Book> matchingBooks = new List<Book>();
            foreach (Book book in books)
            {
                if (KMPSearch(book.Title.ToLower(), pattern.ToLower()))
                {
                    matchingBooks.Add(book);
                }
            }

            return matchingBooks;
        }

        public static bool KMPSearch(string text, string pattern)
        {
            int n = text.Length;
            int m = pattern.Length;

            int[] lps = ComputeLPSArray(pattern, m);

            int i = 0; // index for text[]
            int j = 0; // index for pattern[]
            while (i < n)
            {
                if (pattern[j] == text[i])
                {
                    j++;
                    i++;
                }
                if (j == m)
                {
                    return true;
                }
                else if (i < n && pattern[j] != text[i])
                {
                    if (j != 0)
                    {
                        j = lps[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return false;
        }

        public static int[] ComputeLPSArray(string pattern, int m)
        {
            int[] lps = new int[m];
            int len = 0;
            int i = 1;
            lps[0] = 0;

            while (i < m)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }
            return lps;
        }
    }
}
