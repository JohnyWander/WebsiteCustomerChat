using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
namespace SelfSignedPfxGenerator
{
    class crypto
    {

        public byte[] GenerateSelfSignedCertificatePfx(string subjectName, string password)
        {
            // Generate Key Pair
            var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

            // Generate Self-Signed Certificate
            var certificateGenerator = new X509V3CertificateGenerator();
            var serialNumber = BigInteger.ProbablePrime(120, new Random());
            certificateGenerator.SetSerialNumber(serialNumber);
            certificateGenerator.SetSubjectDN(new X509Name(subjectName));
            certificateGenerator.SetIssuerDN(new X509Name(subjectName)); // Self-signed
            certificateGenerator.SetNotBefore(DateTime.UtcNow.Date);
            certificateGenerator.SetNotAfter(DateTime.UtcNow.Date.AddYears(10));
            certificateGenerator.SetPublicKey(keyPair.Public);
            var signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", keyPair.Private);
            var certificate = certificateGenerator.Generate(signatureFactory);

            // Create PFX (PKCS#12) container
            var store = new Pkcs12StoreBuilder().Build();
            var friendlyName = subjectName;
            var certificateEntry = new X509CertificateEntry(certificate);

            store.SetCertificateEntry(friendlyName, certificateEntry);
            store.SetKeyEntry(friendlyName, new AsymmetricKeyEntry(keyPair.Private), new[] { certificateEntry });

            using (var ms = new MemoryStream())
            {
                store.Save(ms, password.ToCharArray(), new SecureRandom(new CryptoApiRandomGenerator()));
                return ms.ToArray();
            }
        }
    }
}
