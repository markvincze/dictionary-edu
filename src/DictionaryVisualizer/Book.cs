using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryVisualizer
{
    public class Book
    {
        private readonly string author;
        private readonly string title;

        public Book(string author, string title)
        {
            this.author = author;
            this.title = title;
        }

        public string Author { get { return author; } }

        public string Title { get { return title; } }

        public override string ToString()
        {
            return String.Format("{0} - {1}", author, title);
        }
    }
}
