/*-
 * #%L
 * Smart ID sample Java client
 * %%
 * Copyright (C) 2018 SK ID Solutions AS
 * %%
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * #L%
 */

using SK.SmartId.Rest.Dao;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SK.SmartId.IntegrationTests
{
    public class SmartIdIntegrationTest
    {
        private const string HOST_URL = "https://sid.demo.sk.ee/smart-id-rp/v2/";
        private const string RELYING_PARTY_UUID = "00000000-0000-0000-0000-000000000000";
        private const string RELYING_PARTY_NAME = "DEMO";
        private const string DOCUMENT_NUMBER = "PNOEE-30303039914-5QSV-Q";
        private const string DATA_TO_SIGN = "Well hello there!";
        private const string CERTIFICATE_LEVEL_QUALIFIED = "QUALIFIED";
        private readonly SmartIdClient client;

        public SmartIdIntegrationTest()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            var httpClient = new HttpClient(httpClientHandler);

            client = new SmartIdClient
            {
                RelyingPartyUUID = RELYING_PARTY_UUID,
                RelyingPartyName = RELYING_PARTY_NAME
            };
            client.SetHostUrl(HOST_URL);
            client.SetConfiguredClient(httpClient);
        }

        [Fact]
        public async Task GetCertificate_bySemanticsIdentifier()
        {
            SmartIdCertificate certificateResponse = await client
                .GetCertificate()
                .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                    .WithRelyingPartyName(RELYING_PARTY_NAME)
                    .WithSemanticsIdentifier(new SemanticsIdentifier(SemanticsIdentifier.IdentityType.PNO, SemanticsIdentifier.CountryCode.EE, "30303039914"))
                .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                    .WithNonce("012345678901234567890123456789")
                    .FetchAsync();

            Assert.Equal("PNOEE-30303039914-5QSV-Q", certificateResponse.DocumentNumber);
            Assert.Equal("QUALIFIED", certificateResponse.CertificateLevel);
            Assert.Equal("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", Convert.ToBase64String(certificateResponse.Certificate.RawData));
        }

        [Fact]
        public async Task getCertificate_bySemanticsIdentifier_latvian()
        {
            SmartIdCertificate certificateResponse = await client
                .GetCertificate()
                .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                    .WithRelyingPartyName(RELYING_PARTY_NAME)
                    .WithSemanticsIdentifier(new SemanticsIdentifier(SemanticsIdentifier.IdentityType.PNO, SemanticsIdentifier.CountryCode.LV, "030403-10075"))
                .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                    .WithNonce("012345678901234567890123456789")
                    .FetchAsync();

            Assert.Equal("PNOLV-030403-10075-ZH4M-Q", certificateResponse.DocumentNumber);
            Assert.Equal("QUALIFIED", certificateResponse.CertificateLevel);
            Assert.Equal("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", Convert.ToBase64String(certificateResponse.Certificate.RawData));
        }

        [Fact]
        public async Task getCertificate_bySemanticsIdentifier_dateOfBirthParsedFromFieldInCertificate()
        {
            SmartIdCertificate certificateResponse = await client
                .GetCertificate()
                .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                    .WithRelyingPartyName(RELYING_PARTY_NAME)
                    .WithSemanticsIdentifier(new SemanticsIdentifier(SemanticsIdentifier.IdentityType.PNO, SemanticsIdentifier.CountryCode.LV, "329999-99901"))
                .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                    .WithNonce("012345678901234567890123456789")
                    .FetchAsync();

            Assert.Equal("PNOLV-329999-99901-AAAA-Q", certificateResponse.DocumentNumber);
            Assert.Equal("QUALIFIED", certificateResponse.CertificateLevel);
            Assert.Equal("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", Convert.ToBase64String(certificateResponse.Certificate.RawData));

            AuthenticationIdentity identity = AuthenticationResponseValidator.ConstructAuthenticationIdentity(certificateResponse.Certificate);
            Assert.NotNull(identity.DateOfBirth);
            Assert.Equal(new DateTime(1903, 3, 3), identity.DateOfBirth);
        }

        [Fact]
        public async Task GetCertificateEE_byDocumentNumber()
        {
            SmartIdCertificate certificateResponse = await client
                .GetCertificate()
                .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                .WithRelyingPartyName(RELYING_PARTY_NAME)
                .WithDocumentNumber(DOCUMENT_NUMBER)
                .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                .FetchAsync();

            Assert.Equal("PNOEE-30303039914-5QSV-Q", certificateResponse.DocumentNumber);
            Assert.Equal("QUALIFIED", certificateResponse.CertificateLevel);
            Assert.Equal("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", Convert.ToBase64String(certificateResponse.Certificate.RawData));
        }

        [Fact]
        public async Task GetCertificateAndSignHash_withValidRelayingPartyAndUser_successfulCertificateRequestAndDataSigning()
        {
            SmartIdCertificate certificateResponse = await client
                 .GetCertificate()
                 .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                 .WithRelyingPartyName(RELYING_PARTY_NAME)
                 .WithDocumentNumber("PNOLT-30303039914-PBZK-Q")
                 .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                 .FetchAsync();

            AssertCertificateChosen(certificateResponse);

            string documentNumber = certificateResponse.DocumentNumber;
            SignableData dataToSign = new SignableData(Encoding.UTF8.GetBytes(DATA_TO_SIGN));

            SmartIdSignature signature = await client
                 .CreateSignature()
                 .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                 .WithRelyingPartyName(RELYING_PARTY_NAME)
                 .WithDocumentNumber(documentNumber)
                 .WithSignableData(dataToSign)
                 .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                 .WithAllowedInteractionsOrder(
                         new List<Interaction> { Interaction.DisplayTextAndPIN("012345678901234567890123456789012345678901234567890123456789") }
                 )
                 .SignAsync();

            AssertSignatureCreated(signature);
        }

        [Fact]
        public async Task Authenticate_withValidUserAndRelayingPartyAndHash_successfulAuthentication()
        {
            AuthenticationHash authenticationHash = AuthenticationHash.GenerateRandomHash();
            Assert.NotNull(authenticationHash.CalculateVerificationCode());

            SmartIdAuthenticationResponse authenticationResponse = await client
                 .CreateAuthentication()
                 .WithRelyingPartyUUID(RELYING_PARTY_UUID)
                 .WithRelyingPartyName(RELYING_PARTY_NAME)
                 .WithDocumentNumber(DOCUMENT_NUMBER)
                 .WithAuthenticationHash(authenticationHash)
                 .WithCertificateLevel(CERTIFICATE_LEVEL_QUALIFIED)
                 .WithAllowedInteractionsOrder(new List<Interaction> { Interaction.DisplayTextAndPIN("Log in to internet bank?") })
                 .AuthenticateAsync();

            AssertAuthenticationResponseCreated(authenticationResponse, authenticationHash.HashInBase64);

            AuthenticationResponseValidator authenticationResponseValidator = new AuthenticationResponseValidator();
            AuthenticationIdentity authenticationIdentity = authenticationResponseValidator.Validate(authenticationResponse);

            Assert.Equal("QUALIFIED OK", authenticationIdentity.GivenName);
            Assert.Equal("TESTNUMBER", authenticationIdentity.Surname);
            Assert.Equal("30303039914", authenticationIdentity.IdentityNumber);
            Assert.Equal("EE", authenticationIdentity.Country);
        }

        private void AssertSignatureCreated(SmartIdSignature signature)
        {
            Assert.NotNull(signature);
            Assert.False(string.IsNullOrEmpty(signature.ValueInBase64));
        }

        private void AssertCertificateChosen(SmartIdCertificate certificateResponse)
        {
            Assert.NotNull(certificateResponse);
            Assert.False(string.IsNullOrEmpty(certificateResponse.DocumentNumber));
            Assert.NotNull(certificateResponse.Certificate);
        }

        private void AssertAuthenticationResponseCreated(SmartIdAuthenticationResponse authenticationResponse, string expectedHashToSignInBase64)
        {
            Assert.NotNull(authenticationResponse);
            Assert.False(string.IsNullOrEmpty(authenticationResponse.EndResult));
            Assert.Equal(expectedHashToSignInBase64, authenticationResponse.SignedHashInBase64);
            Assert.False(string.IsNullOrEmpty(authenticationResponse.SignatureValueInBase64));
            Assert.NotNull(authenticationResponse.Certificate);
            Assert.NotNull(authenticationResponse.CertificateLevel);
        }
    }
}