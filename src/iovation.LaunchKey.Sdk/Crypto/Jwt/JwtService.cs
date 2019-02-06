using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Time;
using Newtonsoft.Json.Linq;

namespace iovation.LaunchKey.Sdk.Crypto.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IUnixTimeConverter _timestampConverter;
        private readonly string _apiIdentifier;
        private readonly Dictionary<string, RSA> _privateKeys;
        private readonly string _currentPrivateKeyId;
        private readonly int _requestExpireSeconds;
        private readonly string[] AcceptableAlgorithms = { "RS256", "RS384", "RS512" };

        public JwtService(
            IUnixTimeConverter timestampConverter,
            string apiIdentifier,
            Dictionary<string, RSA> privateKeys,
            string currentPrivateKeyId,
            int requestExpireSeconds)
        {
            _timestampConverter = timestampConverter;
            _apiIdentifier = apiIdentifier;
            _privateKeys = privateKeys;
            _currentPrivateKeyId = currentPrivateKeyId;
            _requestExpireSeconds = requestExpireSeconds;
        }

        public string Encode(
            string jti,
            string issuer,
            string subject,
            DateTime currentTime,
            string method,
            string path,
            string contentHashAlgorithm,
            string contentHash)
        {
            var now = _timestampConverter.GetUnixTimestamp(currentTime);
            var expire = now + _requestExpireSeconds;
            var requestPayload = new Dictionary<string, object>
            {
                {"meth", method},
                {"path", path}
            };
            if (contentHashAlgorithm != null)
            {
                requestPayload["func"] = contentHashAlgorithm;
                requestPayload["hash"] = contentHash;
            }

            var payload = new Dictionary<string, object>
            {
                {"jti", jti},
                {"iss", issuer},
                {"sub", subject},
                {"aud", _apiIdentifier},
                {"iat", now},
                {"nbf", now},
                {"exp", expire},
                {"request", requestPayload}
            };
            return Jose.JWT.Encode(
                payload: payload,
                key: _privateKeys[_currentPrivateKeyId],
                algorithm: Jose.JwsAlgorithm.RS256,
                extraHeaders: new Dictionary<string, object>
                {
                    {"kid", _currentPrivateKeyId}
                }
            );
        }

        public JwtClaims Decode(RSA publicKey, string expectedAudience, string expectedTokenId, DateTime currentTime, string jwt)
        {
            var headers = Jose.JWT.Headers(jwt);

            if (!headers.ContainsKey("alg")) throw new JwtError("alg is missing");

            var alg = headers["alg"] as string;
            if (alg == null) throw new JwtError("alg is not a string");

            if (!AcceptableAlgorithms.Contains(alg)) throw new JwtError("alg is not of expected set: RS256, RS384 or RS512");

            try
            {
                var decoded = Jose.JWT.Decode(jwt, publicKey);
                var jwtClaims = JwtClaims.FromJson(decoded);

                if (jwtClaims.Audience != expectedAudience)
                    throw new JwtError($"Validation failed -- audience was [{jwtClaims.Audience}], expected [{expectedAudience}]");

                if (expectedTokenId != null && jwtClaims.TokenId != expectedTokenId)
                    throw new JwtError($"Validation failed -- token ID was [{jwtClaims.TokenId}], expected [{expectedTokenId}]");

                if (jwtClaims.ExpiresAt == default(DateTime))
                    throw new JwtError($"Validation failed -- expiration is missing");

                if (jwtClaims.IssuedAt == default(DateTime))
                    throw new JwtError($"Validation failed -- issued at is missing");

                if (jwtClaims.NotBefore == default(DateTime))
                    throw new JwtError($"Validation failed -- not before is missing");

                if (Math.Abs(jwtClaims.IssuedAt.Subtract(currentTime).TotalSeconds) > _requestExpireSeconds)
                    throw new JwtError($"Validation failed -- token has expired");

                return jwtClaims;
            }
            catch (JwtError)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new JwtError("Error while decoding the JWT object, see inner exception", ex);
            }
        }

        public JwtData GetJWTData(string jwt)
        {
            try
            {
                var headers = Jose.JWT.Headers(jwt);
                var payload = Jose.JWT.Payload(jwt);

                if (headers["kid"] == null)
                    throw new JwtError("Key id missing from jwt header");

                var o = JObject.Parse(payload);

                return new JwtData(
                    o["iss"].Value<string>(),
                    o["sub"].Value<string>(),
                    o["aud"].Value<string>(),
                    headers["kid"] as string
                );
            }
            catch (JwtError)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new JwtError("Error while decoding the JWT object, see inner exception", ex);
            }
        }
    }
}