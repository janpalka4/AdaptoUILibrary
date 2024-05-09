using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AdaptoUILibrary.Classes
{
    public interface IElementMeasurementService
    {
        public Task<double> GetElementWidth(ElementReference element);
        public Task<double> GetElementHeight(ElementReference element);
    }

    public class ElementMeasurementService : IElementMeasurementService
    {
        private readonly IJSRuntime jSRuntime;

        public ElementMeasurementService(IJSRuntime jSRuntime) 
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task<double> GetElementWidth(ElementReference element) => await jSRuntime.InvokeAsync<double>("GetElementWidth", element);
        public async Task<double> GetElementHeight(ElementReference element) => await jSRuntime.InvokeAsync<double>("GetElementHeight", element);
    }
}
