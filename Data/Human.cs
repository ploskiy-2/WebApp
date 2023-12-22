using System.ComponentModel.DataAnnotations;

namespace WebAppForBD.Data
{
    public class Human
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public Human(string cur_name)
        {
            name = cur_name;
        }
        public Human()
        {
        }
        public HashSet<Movie> currentMoviesConnection { get; set; } = new HashSet<Movie>();

        public string Print(IEnumerable<Movie> movies)
        {
            string output = "";
            output += "<br>Фильмы, в которых он/она принял/а участие:";
            foreach (var t in movies)
            {
                output += $"<br><a href='/movie?name={t.tittle}'>{t.tittle}</a>, ";
            }
            return output;
        }    
    }

}
