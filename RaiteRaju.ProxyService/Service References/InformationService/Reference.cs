﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RaiteRaju.ProxyService.InformationService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="InformationService.InformationServiceInterface")]
    public interface InformationServiceInterface {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InformationServiceInterface/FetchName", ReplyAction="http://tempuri.org/InformationServiceInterface/FetchNameResponse")]
        string FetchName();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InformationServiceInterface/FetchName", ReplyAction="http://tempuri.org/InformationServiceInterface/FetchNameResponse")]
        System.Threading.Tasks.Task<string> FetchNameAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface InformationServiceInterfaceChannel : RaiteRaju.ProxyService.InformationService.InformationServiceInterface, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InformationServiceInterfaceClient : System.ServiceModel.ClientBase<RaiteRaju.ProxyService.InformationService.InformationServiceInterface>, RaiteRaju.ProxyService.InformationService.InformationServiceInterface {
        
        public InformationServiceInterfaceClient() {
        }
        
        public InformationServiceInterfaceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InformationServiceInterfaceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InformationServiceInterfaceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InformationServiceInterfaceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string FetchName() {
            return base.Channel.FetchName();
        }
        
        public System.Threading.Tasks.Task<string> FetchNameAsync() {
            return base.Channel.FetchNameAsync();
        }
    }
}
