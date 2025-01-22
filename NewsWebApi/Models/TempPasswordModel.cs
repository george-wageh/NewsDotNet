namespace NewsWebApi.Models
{
    public class TempPasswordModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Date { get; set; }
    }
}
