<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="InfinitespaceStudios.RemoteEffectService" generation="1" functional="0" release="0" Id="4b2d41a9-a85c-4aae-8399-e11e21aa2a4e" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="InfinitespaceStudios.RemoteEffectServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="PipelineRole:secure" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/LB:PipelineRole:secure" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|PipelineRole:pipelinessl" defaultValue="">
          <maps>
            <mapMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/MapCertificate|PipelineRole:pipelinessl" />
          </maps>
        </aCS>
        <aCS name="PipelineRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/MapPipelineRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="PipelineRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/MapPipelineRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:PipelineRole:secure">
          <toPorts>
            <inPortMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRole/secure" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCertificate|PipelineRole:pipelinessl" kind="Identity">
          <certificate>
            <certificateMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRole/pipelinessl" />
          </certificate>
        </map>
        <map name="MapPipelineRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapPipelineRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="PipelineRole" generation="1" functional="0" release="0" software="C:\Users\dean\Desktop\InfinitespaceStudios.Pipeline\InfinitespaceStudios.PipelineService\InfinitespaceStudios.PipelineService\csx\Release\roles\PipelineRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="secure" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRole/pipelinessl" />
                </certificate>
              </inPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;PipelineRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;PipelineRole&quot;&gt;&lt;e name=&quot;secure&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0pipelinessl" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRole/pipelinessl" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="pipelinessl" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="PipelineRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="PipelineRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="PipelineRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e36db58d-3660-48b3-9639-88c516dc18a1" ref="Microsoft.RedDog.Contract\ServiceContract\InfinitespaceStudios.RemoteEffectServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="00361572-f58e-42a0-a772-97efb9e813bf" ref="Microsoft.RedDog.Contract\Interface\PipelineRole:secure@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/InfinitespaceStudios.RemoteEffectService/InfinitespaceStudios.RemoteEffectServiceGroup/PipelineRole:secure" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>