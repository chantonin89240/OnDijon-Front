namespace FrontMDD.Entities
{
    public class Profil
    {
        public int? Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public Guid? Guid { get; set; }

        public Profil()
        {

        }

        public Profil(int idProfil)
        {
            Id = idProfil;
        }

        public Profil(int idProfil, string nom, string prenom, Guid guid)
        {
            this.Id = idProfil;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Guid = guid;

        }


    }
}