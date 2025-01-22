namespace NewsWebApi.Models
{
    public class SectionModel
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; }
        public ICollection<NewModel> News { get; set; }
    }
}
