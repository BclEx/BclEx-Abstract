#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using Xunit;
namespace System.Patterns.ReleaseManagement
{
    public class ReleaseManagementExtensionsTests
    {
        [Fact]
        public void DeploymentEnvironment_ToShortName_Return_ProperValues()
        {
            Assert.Equal("proof", DeploymentEnvironment.ProofOfConcept.ToShortName());
            Assert.Equal("local", DeploymentEnvironment.Local.ToShortName());
            Assert.Equal("develop", DeploymentEnvironment.Development.ToShortName());
            Assert.Equal("alpha", DeploymentEnvironment.AlphaTesting.ToShortName());
            Assert.Equal("beta", DeploymentEnvironment.BetaTesting.ToShortName());
            Assert.Equal("prod", DeploymentEnvironment.Production.ToShortName());
        }

        [Fact]
        public void DeploymentEnvironment_IsExternalDeployment_Return_ProperValues()
        {
            Assert.False(DeploymentEnvironment.ProofOfConcept.IsExternalDeployment());
            Assert.False(DeploymentEnvironment.Local.IsExternalDeployment());
            Assert.False(DeploymentEnvironment.Development.IsExternalDeployment());
            Assert.False(DeploymentEnvironment.AlphaTesting.IsExternalDeployment());
            Assert.True(DeploymentEnvironment.BetaTesting.IsExternalDeployment());
            Assert.True(DeploymentEnvironment.Production.IsExternalDeployment());
        }

        [Fact]
        public void DeploymentEnvironment_ToCode_Return_ProperValues()
        {
            Assert.Equal("X", DeploymentEnvironment.ProofOfConcept.ToCode());
            Assert.Equal("Z", DeploymentEnvironment.Local.ToCode());
            Assert.Equal("D", DeploymentEnvironment.Development.ToCode());
            Assert.Equal("A", DeploymentEnvironment.AlphaTesting.ToCode());
            Assert.Equal("B", DeploymentEnvironment.BetaTesting.ToCode());
            Assert.Equal("P", DeploymentEnvironment.Production.ToCode());
        }

        [Fact]
        public void DevelopmentStage_ToCode_Return_ProperValues()
        {
            Assert.Equal("D", DevelopmentStage.PreAlpha.ToCode());
            Assert.Equal("A", DevelopmentStage.Alpha.ToCode());
            Assert.Equal("B", DevelopmentStage.Beta.ToCode());
            Assert.Equal("P", DevelopmentStage.Release.ToCode());
        }
    }
}
