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

using System.Text;
using Xunit;

namespace SK.SmartId
{
    public class VerificationCodeCalculatorTest
    {
        [Theory]
        [InlineData("7712", "Hello World!")]
        [InlineData("4612", "Hedgehogs – why can't they just share the hedge?")]
        [InlineData("7782", "Go ahead, make my day.")]
        [InlineData("1464", "You're gonna need a bigger boat.")]
        [InlineData("4240", "Say 'hello' to my little friend!")]
        public void calculateCorrectVerificationCode(string verificationCode, string dataString)
        {
            byte[] data = Encoding.UTF8.GetBytes(dataString);
            byte[] hash = DigestCalculator.CalculateDigest(data, HashType.SHA256);
            Assert.Equal(verificationCode, VerificationCodeCalculator.Calculate(hash));
        }
    }
}