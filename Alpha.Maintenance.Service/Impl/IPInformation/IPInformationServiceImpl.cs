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

        public IPInfo GetIPInfoFromTableMini(string ip)
        {
            var isIpV6 = IsIPV6(ip);
            var ipNumber = isIpV6
                ? ConvertIPV6ToNumber(ip)
                : ConvertIPV4ToNumber(ip);

            return ipInformationRepository.GetIPInfoFromTableMini(new GetIPInfoFromTableMiniCriteria(isIpV6, ipNumber));
        }

        private static bool IsIPV6(string ip)
        {
            IPAddress address;

            return IPAddress.TryParse(ip, out address)
                && address.AddressFamily == AddressFamily.InterNetworkV6;
        }

        private static uint ConvertIPV4ToNumber(string ipAddress)
        {
            IPAddress parsedIpAddress;

            if (!IPAddress.TryParse(ipAddress, out parsedIpAddress))
            {
                return 0;
            }

            var addressBytes = parsedIpAddress.GetAddressBytes();
            Array.Reverse(addressBytes);

            return BitConverter.ToUInt32(addressBytes, 0);
        }

        private static long ConvertIPV6ToNumber(string ipAddress)
        {
            var addrBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            if (IsLittleEndian)
            {
                var byteList = new List<byte>(addrBytes);
                byteList.Reverse();
                addrBytes = byteList.ToArray();
            }

            var ipNum = BitConverter.ToInt64(addrBytes, 8);
            ipNum <<= 64;
            ipNum += BitConverter.ToInt64(addrBytes, 0);

            return ipNum;
        }
    }
}