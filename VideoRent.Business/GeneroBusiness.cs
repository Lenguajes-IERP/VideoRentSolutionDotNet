using VideoRent.Data;
using VideoRent.Domain;

namespace VideoRent.Business
{
    public class GeneroBusiness
    {
        private GeneroData generoData;

        public GeneroBusiness(String connectionString)
        {
            this.generoData = new GeneroData(connectionString);
        }

        public async Task<IEnumerable<Genero>> GetGeneros()
        {
            return await generoData.GetGeneros();
        }
    }
}
