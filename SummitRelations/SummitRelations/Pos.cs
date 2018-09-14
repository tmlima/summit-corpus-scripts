
namespace SummitRelations
{
    public class Pos
    {
        public string Text { get; private set; }
        public string Id { get; private set; }
        public char Number { get; set; }
        public string Canon { get; set; }

        public Pos(string id, string text)
        {
            this.Id = id;
            this.Text = text;
        }
    }
}
