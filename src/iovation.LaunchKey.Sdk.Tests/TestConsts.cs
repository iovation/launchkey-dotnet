using System;
using iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests
{
    public class TestConsts
    {
        public static readonly Guid DefaultAuthenticationId = new Guid("00000000-0000-0000-0000-000000000007");
        public static readonly Guid DefaultDirectoryId = new Guid("00000000-0000-0000-0000-000000000002");
        public static readonly Guid DefaultServiceId = new Guid("00000000-0000-0000-0000-000000000009");
        public static readonly Guid DefaultOrgId = new Guid("00000000-0000-0000-0000-000000000012");
        public static readonly EntityIdentifier DefaultServiceEntity = new EntityIdentifier(EntityType.Service, TestConsts.DefaultServiceId);
        public static readonly EntityIdentifier DefaultOrganizationEntity = new EntityIdentifier(EntityType.Organization, TestConsts.DefaultOrgId);
        public static readonly EntityIdentifier DefaultDirectoryEntity = new EntityIdentifier(EntityType.Directory, TestConsts.DefaultDirectoryId);
        public static readonly Guid DefaultDeviceId = new Guid("00000000-0000-0000-0000-000000000005");
        public static readonly DateTime DefaultTime = new DateTime(2015, 1, 1);
        public static readonly string DefaultWebhookUrl = "https://a.webhook.url/path";

        public static readonly string DefaultPrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEA7fEWbTsSs1+QYFP2pGJFyFb/fPO90jGV5rRE5FRbdinH7KDA
Q1CbX7Tr7lalIWrDzIw3u0BCN33qoXRIcXS1aSsodf4OOkATZiWSfJs7u4BZowT4
eNLq9iDjrKG7+n4kh83hKobVP6+ftUVqzvF+nlddZCoVEKGO8hiEEJcULJ+dJtq/
HE97M6pwvWb5s1+ypVwx9ql3JcIlCKAtFYDCgX00CGgHZzPF/AnSh16jXCgrgf9o
QQvY9ZaIPCnzawJDNfI2s7MeWYCItxypO8TmT+UFcA5oqu4D4I7In+QICIuZIm1j
QxDVXiNVgcUFeAFSiBAU5WEvPT+DGPMI5xZxDwIDAQABAoIBAHCfaGoTR+q3FPND
Sr7L9RO6eft+sx5Gchlcwi8A7rmjVQnfnKFACgrm67VINaj3i+3JgKeU5sK1StVY
4OEyyJsa3m/a7IkNwyDaL9fi8gbx3XuX4rPtwWD2eE1B0GPkSQ4umKE0aMNYMVPv
o3qIO9Jer8m1UXspvStCB2CC4f2sNWE/XqdZuczBLYnzKnr3jW1/AZkZ1QgVZQZ2
NzAtZ26yzdnqP1rW7FovzaT0xeRUNdB4iFcVkCnpO9CvSlrBisNP/eG6qOCiJxpa
UjtAH7+YPHLTG6qfOLfdHiNAAJb4rYVw/k4lngd1GEPsWDLwkYAG8jDtBM5Nxz8j
z9cFhEECgYEA+gtuYF19wMQHZgrxH7/+ISU1a1vsQ9CxxChmR3qXmRPV32Kc0YMT
TpQqIXWZMin5RVaxIBN1gxb9BVor7Z/z4v5zVx2EhqKFXX8SckT7dv4tyYMg6Tft
VlpqbbHxK+gCDh3L8KtNQJ8a2cmRLkwF+IHcHKaPsCUBhbrEvlkArxcCgYEA85vc
30z2GMtAMCOEX2V7tbR4YukUKrNbNIKld0u2bV0yGqyoxQSGWEzemoy3lkqPmoCe
qC1XKJ2n0uKxYQ02AmzzW/Uu3kPYznyobnFFZjXjPMIupBO+AEo+tuUd+Tfdlbqq
VGUXwe3qeoXSn0pHkgGDAz01E4R/LG8XF8s1yMkCgYEAyiK7lAOASXkvUfq+eqBG
3Jyr1qJ1GU14p87RAC/GpeU3HGMnyudfkEKO3IWo1ri/3qH6hqe0c8j9unnu0SZh
jruMmnwuSnjPv4mea+oAL23DrgfnbyHbJ9fn+c3D3W6tWqQT3fddeEQm/LDKQNcM
bJzuR+sOdaM029rkecyRlPkCgYB+5/p3RaOKpQ+KRGZoP/jjXuG8PUnpOMhRoaHL
dODTNlKsvLeq80F9bIYmoxncrHkE9u4wFHasTP0pijj3oyc4ukNI64B//35Ji30/
E7kglwALHemoRjSb+BGVu+QSeXzwzG6BSqzGtUeKjn57xvlj4W+71z34LfUGU9UG
6zDYqQKBgHTiwtgGFlX83uLlWnBSoTbtidouKVcuoS3EzzzrvGjEwqFAxZ/1BZw+
lcal1kPurB1lu6lp4ElN5D2vQ/djtJykA8/ELPIk3InXZGtE+kZodlGtVyS/r742
aTNSDG8Bvrs4PkTJ4DWBisxmkYYvIQKjStezao06x2Wkr7CS8gd3
-----END RSA PRIVATE KEY-----";

        public static readonly string DefaultPublicKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA7fEWbTsSs1+QYFP2pGJF
yFb/fPO90jGV5rRE5FRbdinH7KDAQ1CbX7Tr7lalIWrDzIw3u0BCN33qoXRIcXS1
aSsodf4OOkATZiWSfJs7u4BZowT4eNLq9iDjrKG7+n4kh83hKobVP6+ftUVqzvF+
nlddZCoVEKGO8hiEEJcULJ+dJtq/HE97M6pwvWb5s1+ypVwx9ql3JcIlCKAtFYDC
gX00CGgHZzPF/AnSh16jXCgrgf9oQQvY9ZaIPCnzawJDNfI2s7MeWYCItxypO8Tm
T+UFcA5oqu4D4I7In+QICIuZIm1jQxDVXiNVgcUFeAFSiBAU5WEvPT+DGPMI5xZx
DwIDAQAB
-----END PUBLIC KEY-----";
    }
}
