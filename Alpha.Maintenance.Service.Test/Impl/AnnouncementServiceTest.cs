namespace Alpha.Maintenance.Service.Test.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Core.Model;
    using Interface.Announcement;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;
    using Repository.Impl;
    using Service.Impl.Announcement;

    [TestFixture]
    public class AnnouncementServiceTest
    {
        private SpecificationFactory specificationFactory;
        private AnnouncementRepository announcementRepository;

        private AnnouncementServiceImpl announcementServiceImpl;
        private BaseAnnouncementService baseAnnouncementService;
       
        private IEnumerable<string> languages;
        private IEnumerable<string> offeredSpecifications;
        private Announcement announcementSample;

        [SetUp]
        public void SetUp()
        {
            specificationFactory = Substitute.For<SpecificationFactory>();
            announcementRepository = Substitute.For<AnnouncementRepository>();
            announcementRepository = Substitute.For<AnnouncementRepository>();
            baseAnnouncementService = Substitute.For<BaseAnnouncementService>();

            announcementSample = BuildAnnouncementSample();
            languages = new List<string> {
                                            "en-US",
                                            "zh-CN",
                                            "zh-TW",
                                            "ja-JP",
                                            "ko-KR",
                                            "th-TH",
                                            "vi-VN"
                                        };

            offeredSpecifications = new List<string> {
                                                        "Currencies",
                                                        "User Ids",
                                                        "User Level",
                                                        "Sites",
                                                        "Products",
                                                        "Not Synced Casino"
                                                       };

            announcementServiceImpl = new AnnouncementServiceImpl(
                                                specificationFactory, 
                                                announcementRepository,
                                                baseAnnouncementService,
                                                languages,
                                                offeredSpecifications);
        }

        [Test]
        public void GetAnnouncements_CallGetAnnouncementsRepository()
        {
            // Arrange
            var selectAnnouncementsCriteria = new SelectAnnouncementsCriteria();

            // Act
            announcementServiceImpl.GetAnnouncements(selectAnnouncementsCriteria);

            // Assert
            announcementRepository.Received().GetAnnouncements(Arg.Any<SelectAnnouncementsCriteria>());
        }

        [Test]
        public void GetAnnouncementById_CallGetAnnouncementByIdRepository()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            announcementRepository.GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>()).Returns(announcementSample);

            // Act
            announcementServiceImpl.GetAnnouncementById(selectAnnouncementByIdCriteria);

            // Assert
            announcementRepository.Received().GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>());
        }

        [Test]
        public void GetAnnouncementById_CallCorrectContentFunction()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            announcementRepository.GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>())
                                   .Returns(announcementSample);

            // Act
            announcementServiceImpl.GetAnnouncementById(selectAnnouncementByIdCriteria);

            // Assert
            baseAnnouncementService.Received().CorrectContent(
                Arg.Any<IEnumerable<MessageContent>>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void InsertOrUpdateAnnouncement_CallInsertOrUpdateAnnouncementRepository()
        {
            // Arrange && Act
            announcementServiceImpl.InsertOrUpdateAnnouncement(announcementSample, ActionType.Update);

            // Assert
            announcementRepository.Received().InsertOrUpdateAnnouncement(Arg.Any<InsertOrUpdateAnnouncementsCriteria>());
        }

        private Announcement BuildAnnouncementSample()
        {
            var announcement = new Announcement();
            announcement.Id = 1;
            announcement.Code = "07032017";
            announcement.Title = "07032017 - Example";
            announcement.IsActive = false;
            announcement.AnnouncementType = AnnouncementType.HeaderPopup;
            announcement.FromDate = new DateTime(2017, 6, 3);
            announcement.ToDate = new DateTime(2017, 7, 20);

            var environments = "Local, UAT, Staging";
            announcement.Environments = environments.Split(",".ToCharArray(),
                                        StringSplitOptions.RemoveEmptyEntries)
                                        .Select(system => system.Trim());

            var systems = "IBCBet, 789";
            announcement.Systems = systems.Split(",".ToCharArray(),
                                        StringSplitOptions.RemoveEmptyEntries)
                                        .Select(system => system.Trim());
            announcement.Ordinal = 4;

            var specs = @"[  
                           {  
                              'Value':[  
                                 1,
                                 2,
                                 3,
                                 4,
                                 5,
                                 6,
                                 7
                              ],
                              'Id':'E4FE1443-C8D0-4E63-A355-08661859295E',
                              'Name':'Products',
                              'Combinable':true,
                              'TypeName':'Alpha.Announcements.Specs.ProductSpecification, Alpha.Announcements, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null',
                              'ConjunctionOperator':0,
                              'SpecificationOperator':1,
                              'ValueRange':null
                           },
                           {  
                              'Value':[  
                                 'MBC',
                                 ' 123',
                                 ' fdak'
                              ],
                              'Id':'6edc73e0-aa87-4757-96e7-e5ffea0ce06a',
                              'Name':'Sites',
                              'Combinable':true,
                              'TypeName':'Alpha.Announcements.Specs.SiteSpecification, Alpha.Announcements, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null',
                              'ConjunctionOperator':2,
                              'SpecificationOperator':1,
                              'ValueRange':null
                           }
                        ]";

            var contents = @"[  
                                {  
                                    'LanguageKey':'en-US',
                                    'Text':'IDxwPkRlYXIgdmFsdWVkIGFnZW50cyw8L3A+CiAgICAgIDxwPgogICAgICAgIFdlIGFyZSBwbGVhc2VkIHRvIGFubm91bmNlIHRoZSBsYXVuY2ggb2YgbmV3IEJhc2tldGJhbGwgUFQgc2V0dGluZyBuYW1lZCAiT3RoZXJzIi4KICAgICAgPC9wPgogICAgICA8cD4KICAgICAgICBUaGlzIHNldHRpbmcgaXMgdXNlZCBmb3IgdGhlIGZvbGxvd2luZyBiZXQgdHlwZXMgb2YgQmFza2V0YmFsbDoKICAgICAgPC9wPgogICAgICA8cD4KICAgICAgICAtIFdpbm5pbmcgTWFyZ2luIDE0IFdheTxiciAvPgogICAgICAgIC0gV2lubmluZyBNYXJnaW4gMTIgV2F5PGJyIC8+CiAgICAgICAgLSAxSCBXaW5uaW5nIE1hcmdpbiAxMiBXYXk8YnIgLz4KICAgICAgICAtIFF1YXJ0ZXIgWCBXaW5uaW5nIE1hcmdpbiA3IFdheQogICAgICA8L3A+CiAgICAgIDxwPgogICAgICAgIFRoZSBpbml0aWFsIHZhbHVlIG9mIE90aGVycyBQVCBzZXR0aW5nIGlzIGNvcGllZCBmcm9tICJIYW5kaWNhcCIgYW5kIGlzIG5vdyBhdmFpbGFibGUgdG8gY29uZmlndXJlLgogICAgICA8L3A+CiAgICAgIDxwPgogICAgICAgIFRoYW5rIHlvdQogICAgICA8L3A+',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                },
                                {  
                                    'LanguageKey':'zh-CN',
                                    'Text':'IDxwPgogICAgICAgIOaIkeS7rOWcqOatpOWuo+W4g+aOqOWHuuaWsOeahOevrueQg+WNoOaIkOiuvue9ru+8jCDlkI3kuLog4oCc5YW25LuW4oCdCiAgICAgIDwvcD4KICAgICAgPHA+CiAgICAgICAg5q2k6K6+572u5bCG5bqU55So5LqO5Lul5LiL56+u55CD55qE5oqV5rOo57G75Z6L77yaCiAgICAgIDwvcD4KICAgICAgPHA+CiAgICAgICAgLSDlh4Dog5zliIbmlbAxNOmhuTxiciAvPgogICAgICAgIC0g5YeA6IOc5YiG5pWwMTLpobk8YnIgLz4KICAgICAgICAtIOS4iuWNiuWcuuWHgOiDnOWIhuaVsDEy6aG5PGJyIC8+CiAgICAgICAgLSDnrKxY6IqC5YeA6IOc5YiG5pWwN+mhuQogICAgICA8L3A+CiAgICAgIDxwPgogICAgICAgIOKAnOWFtuS7luKAneWOn+Wni+WNoOaIkOiuvue9ruWwhuWkjeWItuKAnOS6mua0suebmOKAneOAguaCqOeOsOWcqOWPr+S7pei/m+ihjOmFjee9rgogICAgICA8L3A+CiAgICAgIDxwPgogICAgICAgIOiwouiwogogICAgICA8L3A+',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                },
                                {  
                                    'LanguageKey':'zh-TW',
                                    'Text':'wqAg5bCK5pWs55qE5Luj55CG77yMPGJyIC8+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIOaIkeWAkeWcqOatpOWuo+W4g+aOqOWHuuaWsOeahOexg+eQg+WNoOaIkOioree9ru+8jCDlkI3ngrog4oCc5YW25LuW4oCdPGJyPsKgwqDCoMKgwqAgPC9wPjxicj7CoMKgwqDCoMKgIDxwPjxicj7CoMKgwqDCoMKgwqDCoCDmraToqK3nva7lsIfmh4nnlKjmlrzku6XkuIvnsYPnkIPnmoTmipXms6jpoZ7lnovvvJo8YnI+wqDCoMKgwqDCoCA8L3A+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIC0g5reo5Yud5YiG5pW4MTTpoIU8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSDmt6jli53liIbmlbgxMumghTxiciAvPjxicj7CoMKgwqDCoMKgwqDCoCAtIOS4iuWNiuWgtOa3qOWLneWIhuaVuDEy6aCFPGJyIC8+PGJyPsKgwqDCoMKgwqDCoMKgIC0g56ysWOevgOeahOa3qOWLneWIhuaVuDfpoIU8YnI+wqDCoMKgwqDCoCA8L3A+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIOKAnOWFtuS7luKAneWOn+Wni+WNoOaIkOioree9ruWwh+W+qeWItuKAnOS6nua0suebpOKAneOAguaCqOePvuWcqOWPr+S7pemAsuihjOmFjee9rjxicj7CoMKgwqDCoMKgIDwvcD48YnI+wqDCoMKgwqDCoCA8cD48YnI+wqDCoMKgwqDCoMKgwqAg6Kyd6KydPGJyPsKgwqDCoMKgwqAgPC9wPg==',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                },
                                {  
                                    'LanguageKey':'ja-JP',
                                    'Text':'wqBEZWFyIHZhbHVlZCBhZ2VudHMsPGJyIC8+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIFdlIGFyZSBwbGVhc2VkIHRvIGFubm91bmNlIHRoZSBsYXVuY2ggb2YgbmV3IEJhc2tldGJhbGwgUFQgc2V0dGluZyBuYW1lZCAiT3RoZXJzIi48YnI+wqDCoMKgwqDCoCA8L3A+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIFRoaXMgc2V0dGluZyBpcyB1c2VkIGZvciB0aGUgZm9sbG93aW5nIGJldCB0eXBlcyBvZiBCYXNrZXRiYWxsOjxicj7CoMKgwqDCoMKgIDwvcD48YnI+wqDCoMKgwqDCoCA8cD48YnI+wqDCoMKgwqDCoMKgwqAgLSBXaW5uaW5nIE1hcmdpbiAxNCBXYXk8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSBXaW5uaW5nIE1hcmdpbiAxMiBXYXk8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSAxSCBXaW5uaW5nIE1hcmdpbiAxMiBXYXk8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSBRdWFydGVyIFggV2lubmluZyBNYXJnaW4gNyBXYXk8YnI+wqDCoMKgwqDCoCA8L3A+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIFRoZSBpbml0aWFsIHZhbHVlIG9mIE90aGVycyBQVCBzZXR0aW5nIGlzIGNvcGllZCBmcm9tICJIYW5kaWNhcCIgYW5kIGlzIG5vdyBhdmFpbGFibGUgdG8gY29uZmlndXJlLjxicj7CoMKgwqDCoMKgIDwvcD48YnI+wqDCoMKgwqDCoCA8cD48YnI+wqDCoMKgwqDCoMKgwqAgVGhhbmsgeW91PGJyPsKgwqDCoMKgwqAgPC9wPg==',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                },
                                {  
                                    'LanguageKey':'ko-KR',
                                    'Text':'wqDCoCBEZWFyIHZhbHVlZCBhZ2VudHMsPGJyIC8+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIFdlIGFyZSBwbGVhc2VkIHRvIGFubm91bmNlIHRoZSBsYXVuY2ggb2YgbmV3IEJhc2tldGJhbGwgUFQgc2V0dGluZyBuYW1lZCAiT3RoZXJzIi48YnI+wqDCoMKgwqDCoCA8L3A+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIFRoaXMgc2V0dGluZyBpcyB1c2VkIGZvciB0aGUgZm9sbG93aW5nIGJldCB0eXBlcyBvZiBCYXNrZXRiYWxsOjxicj7CoMKgwqDCoMKgIDwvcD48YnI+wqDCoMKgwqDCoCA8cD48YnI+wqDCoMKgwqDCoMKgwqAgLSBXaW5uaW5nIE1hcmdpbiAxNCBXYXk8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSBXaW5uaW5nIE1hcmdpbiAxMiBXYXk8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSAxSCBXaW5uaW5nIE1hcmdpbiAxMiBXYXk8YnIgLz48YnI+wqDCoMKgwqDCoMKgwqAgLSBRdWFydGVyIFggV2lubmluZyBNYXJnaW4gNyBXYXk8YnI+wqDCoMKgwqDCoCA8L3A+PGJyPsKgwqDCoMKgwqAgPHA+PGJyPsKgwqDCoMKgwqDCoMKgIFRoZSBpbml0aWFsIHZhbHVlIG9mIE90aGVycyBQVCBzZXR0aW5nIGlzIGNvcGllZCBmcm9tICJIYW5kaWNhcCIgYW5kIGlzIG5vdyBhdmFpbGFibGUgdG8gY29uZmlndXJlLjxicj7CoMKgwqDCoMKgIDwvcD48YnI+wqDCoMKgwqDCoCA8cD48YnI+wqDCoMKgwqDCoMKgwqAgVGhhbmsgeW91PGJyPsKgwqDCoMKgwqAgPC9wPg==',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                },
                                {  
                                    'LanguageKey':'th-TH',
                                    'Text':'IDxwPgogICAgICAgIOC5gOC4o+C4suC4oeC4teC4hOC4p+C4suC4oeC4ouC4tOC4meC4lOC4teC4l+C4teC5iOC4iOC4sOC4m+C4o+C4sOC4geC4suC4qOC4geC4suC4o+C5gOC4m+C4tOC4lOC4leC4seC4p+C4guC4reC4h+C4muC4suC4quC5gOC4geC5h+C4leC5g+C4q+C4oeC5iOC4geC4suC4o+C4leC4seC5ieC4h+C4hOC5iOC4suC4hOC4p+C4suC4oeC5gOC4quC4teC5iOC4ouC4hyAi4Lit4Li34LmI4LiZ4LmGIgogICAgICA8L3A+CiAgICAgIDxwPgogICAgICAgIOC4geC4suC4o+C4leC4seC5ieC4h+C4hOC5iOC4suC4meC4teC5ieC4iOC4sOC4meC4s+C5hOC4m+C5g+C4iuC5ieC4leC5iOC4reC5hOC4m+C4meC4teC5ieC4m+C4o+C4sOC5gOC4oOC4l+C4guC4reC4h+C4geC4suC4o+C5gOC4lOC4tOC4oeC4nuC4seC4meC4muC4suC4quC5gOC4geC4leC4muC4reC4pToKICAgICAgPC9wPgogICAgICA8cD4KICAgICAgICAtIDE0IOC4l+C4suC4h+C4guC4reC4h+C4quC5iOC4p+C4meC4leC5iOC4suC4h+C4iuC4meC4sDxiciAvPgogICAgICAgIC0gMTIg4LiX4Liy4LiH4LiC4Lit4LiH4Liq4LmI4Lin4LiZ4LiV4LmI4Liy4LiH4LiK4LiZ4LiwPGJyIC8+CiAgICAgICAgLSAxMiDguJfguLLguIfguILguK3guIfguKrguYjguKfguJnguJXguYjguLLguIfguIrguJnguLAgKOC4hOC4o+C4tuC5iOC4h+C5geC4o+C4gSk8YnIgLz4KICAgICAgICAtIDcg4LiX4Liy4LiH4LiC4Lit4LiH4Liq4LmI4Lin4LiZ4LiV4LmI4Liy4LiH4LiK4LiZ4LiwICjguITguKfguK3guYDguJXguK3guKPguYwgWCkKICAgICAgPC9wPgogICAgICA8cD4KICAgICAgICDguITguYjguLLguYDguKPguLTguYjguKHguJXguYnguJnguILguK3guIfguIHguLLguKPguJXguLHguYnguIfguITguYjguLLguITguKfguLLguKHguYDguKrguLXguYjguKLguIcgIuC4reC4t+C5iOC4meC5hiIg4LiI4Liw4LiW4Li54LiB4LiE4Lix4LiU4Lil4Lit4LiB4LiI4Liy4LiBICLguKPguLLguITguLLguJXguYjguK3guKPguK3guIciIOC5geC4peC4sOC4quC4suC4oeC4suC4o+C4luC4geC4s+C4q+C4meC4lOC4geC4suC4o+C4leC4seC5ieC4h+C4hOC5iOC4suC5hOC4lOC5ieC5geC4peC5ieC4pwogICAgICA8L3A+CiAgICAgIDxwPgogICAgICAgIOC4guC4reC4muC4hOC4uOC4k+C4hOC5iOC4sAogICAgICA8L3A+',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                },
                                {  
                                    'LanguageKey':'vi-VN',
                                    'Text':'IEvDrW5oIGfhu61pIHF1w70ga2jDoWNoIGjDoG5nLDxiciAvPgogICAgICA8cD4KICAgICAgICBDaMO6bmcgdMO0aSB0csOibiB0cuG7jW5nIHRow7RuZyBiw6FvIG3hu5l0IHRoaeG6v3QgbOG6rXAgUG9zaXRpb24gVGFraW5nIG3hu5tpIGNobyBtw7RuIELDs25nIFLhu5UgbWFuZyB0w6puICJDxrDhu6NjIGtow6FjIi4KICAgICAgPC9wPgogICAgICA8cD4KICAgICAgICBUaGnhur90IGzhuq1wIG3hu5tpIG7DoHkgc+G6vSDEkcaw4bujYyDDoXAgZOG7pW5nIGNobyBjw6FjIGxv4bqhaSBow6xuaCBjxrDhu6NjIHNhdSB0cm9uZyBtw7RuIELDs25nIFLhu5U6CiAgICAgIDwvcD4KICAgICAgPHA+CiAgICAgICAgLSBU4bu3IHPhu5EgdGjhuq9uZyBjw6FjaCBiaeG7h3QgMTQgY2hpw6rMgHU8YnIgLz4KICAgICAgICAtIFThu7cgc+G7kSB0aOG6r25nIGPDoWNoIGJp4buHdCAxMiBjaGnDqsyAdTxiciAvPgogICAgICAgIC0gSGnhu4dwIDEgLSBU4bu3IHPhu5EgdGjhuq9uZyBjw6FjaCBiaeG7h3QgMTIgY2hpw6rMgHU8YnIgLz4KICAgICAgICAtIFbDoW4gY2jGoWkgdGjhu6kgWCAtIFThu7cgc+G7kSB0aOG6r25nIGPDoWNoIGJp4buHdCA3IGNoacOqzIB1CiAgICAgIDwvcD4KICAgICAgPHA+CiAgICAgICAgVGhp4bq/dCBs4bqtcCBt4bq3YyDEkeG7i25oIGNobyBQb3NpdGlvbiBUYWtpbmcgIkPGsOG7o2Mga2jDoWMiIHPhur0gxJHGsOG7o2Mgc2FvIGNow6lwIHThu6sgbG/huqFpIGPGsOG7o2MgSGFuZGljYXAgdsOgIGhp4buHbiDEkcOjIGNobyBwaMOpcCB0aGF5IMSR4buVaS4KICAgICAgPC9wPgogICAgICA8cD4KICAgICAgICBUcsOibiB0cuG7jW5nCiAgICAgIDwvcD4=',
                                    'IsDisplayingByUrl':false,
                                    'Url':''
                                }
                            ]";

            announcement.Specifications = JsonConvert.DeserializeObject<IEnumerable<ISpecification<IParam>>>(
                           specs,
                            new SpecificationConverter());

            announcement.Contents = JsonConvert.DeserializeObject<IEnumerable<MessageContent>>(contents);

            return announcement;
        }
    }
}
