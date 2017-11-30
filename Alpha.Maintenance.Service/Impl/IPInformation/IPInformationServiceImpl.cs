namespace Alpha.Maintenance.Service.Impl.IPInformation
{
    using Core.Model;
    using Interface.IPInformation;
    using Repository.Criteria.IPInformation;
    using Repository.Impl;

    public class IPInformationServiceImpl : IPInformationService
    {
        private readonly IPInformationRepository ipInformactionRepository;

        public IPInformationServiceImpl()
            : this(null)
        {
        }

        public IPInformationServiceImpl(IPInformationRepository ipInformactionRepository = null)
        {
            this.ipInformactionRepository = ipInformactionRepository ?? new IPInformationRepositoryImpl();
        }

        public IPInfo GetIPInfo(string ip) => ipInformactionRepository.GetIPInfo(new GetIPInformationCriteria(ip));
    }
}
