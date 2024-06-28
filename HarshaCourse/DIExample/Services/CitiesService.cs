using ServiceContracts;
namespace Services
{
    public class CitiesService : ICitiesService
    {
        private List<string> _cities;
        public CitiesService()
        {
            _cities = new List<string>() { "New York", "London", "Paris" };
        }   

        public List<string> getCities()
        {
            return _cities;
        }
    }
}
