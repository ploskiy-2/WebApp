using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebAppForBD.Data
{
    public class Movie
    {
        [Key]
        public int id { get; set; }
        public string tittle { get; set; }
        public Movie(string current_tittle)
        {
            tittle = current_tittle;
        }

        public Movie()
        {
        }

        //потенциально похожие фильмы
        [NotMapped]
        public HashSet<string> potentialSimilaryMovie = new HashSet<string>();

        public List<Human> actorsSetConnection { get; set; } = new List<Human>();
        public List<Tag> tagssSetConnection { get; set; } = new List<Tag>();
        public string? director { get; set; }
        public string? movieRating { get; set; }

        [NotMapped]
        public HashSet<string> tagSet = new HashSet<string>();
        [NotMapped]
        public HashSet<string> actorsSet = new HashSet<string>();

        public string Print()
        {
            string output = "";

            output += "<br>Название этого фильма:" + " " + tittle;
            output += "<br>Рейтинг этого фильма:" + " " + movieRating;
            output += "<br>Режиссер этого фильма:" + " " + director;

            return output;

        }
        public string OutputActor(IEnumerable<Human> actorset) 
        {
            var actorNamesStringBuilder = new StringBuilder();
            foreach (var t in actorset)
            {
                //actorNamesStringBuilder.Append($"<a href='/actor?name={t.name}'>{t.name}</a>").Append(" ");
                actorNamesStringBuilder.Append($"<br><a href='/actor?name={t.name}'>{t.name}</a>, ").Append(" ");
            }
            string moviesactorNames = actorNamesStringBuilder.ToString();

            moviesactorNames = "Актеры фильма" + " " + moviesactorNames;

            return moviesactorNames;

          
        }


        public string OutputTags(IEnumerable<Tag> tagset)
        {
            var actorNamesStringBuilder = new StringBuilder();
            foreach (var t in tagset)
            {
                actorNamesStringBuilder.Append($"<br><a href='/tag?name={t.tittle}'>{t.tittle}</a>, ").Append(" ");
            }
            string moviesactorNames = actorNamesStringBuilder.ToString();
            //output += $"<br><a href='/movie?name={t.tittle}'>{t.tittle}</a>, ";

            moviesactorNames = "Теги фильма" + " " + moviesactorNames;

            return moviesactorNames;


        }

    
        public string OutputSimMov(Dictionary<string,double> similaryMovies)
        {
            string output = "";
            foreach (var t in similaryMovies)
            {
                output += string.Join(", ","<br>"+t.Key + " ---- " + t.Value + "  " + "схожести");
            }
            return output;
        }
    }
}

