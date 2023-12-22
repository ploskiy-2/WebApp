namespace WebAppForBD.Services
{
    public class CommunicationService
    {
        public event Action<string> OnTagClicked;

        public void TagClicked(string tagTitle)
        {
            OnTagClicked?.Invoke(tagTitle);
        }
    }
}
