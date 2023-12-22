using System.ComponentModel.DataAnnotations;

namespace WebAppForBD.Data
{
    public class Tag
    {
        [Key]
        public int id { get; set; }
        public string tittle { get; set; }
        public Tag(string cur_tittle)
        {
            tittle = cur_tittle;
        }
        public Tag()
        {
        }
        public HashSet<Movie> currentMoviesConnection { get; set; } = new HashSet<Movie>();

        public string Print(IEnumerable<Movie> movies)
        {
            string output = "";
            output += "<br>Фильмы, помеченные данным тегом:";
            foreach (var t in movies)
            {
                output += $"<br><a href='/movie?name={t.tittle}'>{t.tittle}</a>, ";
            }
            return output;
        }
    }
}
