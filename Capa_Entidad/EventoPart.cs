namespace Capa_Entidad
{
    public class EventoPart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string User { get; set; }
        public int UserUid { get; set; }
        public string State { get; set; }
        public int IdCre { get; set; }
        public List<Guests> listGuests { get; set; }
    }
}
