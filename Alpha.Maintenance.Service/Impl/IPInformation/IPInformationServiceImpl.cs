namespace Alpha.Maintenance.Service.Impl.IPInformation
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using Core.Model;
    using Interface.IPInformation;
    using Repository.Criteria.IPInformation;
    using Repository.Impl;

    public class IPInformationServiceImpl : IPInformationService
    {
        [ThreadStatic]
        private static bool IsLittleEndian = BitConverter.IsLittleEndian;

        private readonly IPInformationRepository ipInformationRepository;

        public IPInformationServiceImpl()
            : this(null)
        {
        }

        public IPInformationServiceImpl(IPInformationRepository ipInformationRepository = null)
        {
            this.ipInformationRepository = ipInformationRepository ?? new IPInformationRepositoryImpl();
        }

        public IPInfo GetIPInfo(string ip) => ipInformationRepository.GetIPInfo(new GetIPInformationCriteria(ip));
        
    }
}