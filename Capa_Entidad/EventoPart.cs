namespace Capa_Entidad
{
    public class EventoPart : EvePartHe
    {
        public int Id { get; set; }
        public int UserUid { get; set; }
        public int IdCre { get; set; }
        public List<Guests> listGuests { get; set; } = new List<Guests>();
    }
}
